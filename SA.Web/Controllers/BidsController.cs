﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA.Application;
using SA.Application.Bid;
using SA.Application.Email;
using SA.Application.Records;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Bids")]
    public class BidsController : BaseController<Bid>
    {
        private readonly IEntityRepository<Record> _recordRepository;
        private readonly IEntityRepository<Auction> _auctionRepository;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IUserEmailFactory _userEmailFactory;

        public BidsController(
            IEntityRepository<Bid> repository,
            IEntityRepository<Record> recordRepository,
            IEntityRepository<Auction> auctionRepository,
            IEntityRepository<User> userRepository,
            IUserEmailFactory userEmailFactory,
            IMapper mapper)
            : base(repository, mapper)
        {
            _recordRepository = recordRepository;
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _userEmailFactory = userEmailFactory;
        }

        [Authorize("read:messages")]
        [HttpGet("{recordId}")]
        [Route("getActualPrice")]
        public async Task<IActionResult> GetActualPrice(int recordId)
        {
            var bids = await _repository
                .GetAllAsync<BidSimpleDto, decimal>(x =>
                    x.RecordId == recordId, x => x.Price);
            return Json(bids.Max(x => x.Price));
        }

        [HttpGet("{id}")]
        [Route("getBidsByRecordId")]
        public ActionResult GetBidsByRecordId(int id)
        {
            var query = _repository.Context.Bids
                .Join(_repository.Context.Users, b => b.UserId, u => u.Id, (b, u) => new { b, u })
                .Join(_repository.Context.Records, bu => bu.b.RecordId, r => r.Id, (bu, r) => new { bu.b, bu.u, r })
                .Where(b => b.b.RecordId == id)
                .OrderByDescending(b => b.b.Price)
                .Select(all => new BidSimpleDto
                {
                    Id = all.b.Id,
                    Price = all.b.Price,
                    RecordId = all.b.RecordId,
                    Created = all.b.Created,
                    UserId = all.b.UserId,
                    UserName = all.u.UserName,
                    RecordValidTo = all.r.ValidTo
                });

            return Json(query);
        }

        [Authorize("admin")]
        [HttpGet("{id}")]
        [Route("getRecordsBidForAdmin")]
        public async Task<IActionResult> GetRecordsBidForAdmin(int id)
            => Json(await _repository
                .GetAllAsync<BidSimpleDto, DateTime?>(
                    x => x.RecordId == id,
                    orderDesc: x => x.Created));

        [HttpGet("{id}")]
        [Route("getRecordsLastBid")]
        public async Task<IActionResult> GetRecordsLastBid(int id)
            => Json(await _mapper.ProjectTo<BidSimpleDto>(_repository.Context.Bids
                    .Where(x => x.RecordId == id)
                    .OrderByDescending(x => x.Created)
                    .Take(1))
                .FirstOrDefaultAsync());

        [Authorize("read:messages")]
        [HttpPost("{recordValidTo}")]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Bid bid)
        {
            var response = new ResponseMessage<BidSimpleDto>
            {
                Status = MessageStatusEnum.Error,
                Entity = new BidSimpleDto
                {
                    Id = bid.Id,
                    Price = bid.Price,
                    RecordId = bid.RecordId,
                    UserId = bid.UserId,
                    Created = bid.Created
                }
            };

            var now = DateTime.Now;
            if (bid == null && bid.RecordId <= 0)
            {
                response.Code = "bidBadRequest";
                return Json(response);
            }

            var record = await _recordRepository.GetOneAsync<RecordTableDto>(x => x.Id == bid.RecordId);

            if (record.NumberOfBids > 0 && record.CurrentPrice >= bid.Price)
            {
                response.Status = MessageStatusEnum.Warning;
                response.Code = "bidOverpaid";
                response.Entity.RecordValidTo = record.ValidTo;
                return Json(response);
            }

            try
            {
                var newBid = await _repository.AddAsync(bid);
                // extend when last bid is less then 5 min. before auction ends.
                if (now.AddMinutes(5) >= record.ValidTo)
                {
                    var recordToUpdate = _recordRepository.Context.Records.First(x => x.Id == record.Id);

                    var extendTo = record.ValidTo.AddMinutes(5);

                    recordToUpdate.ValidTo = extendTo;
                    await _recordRepository.Context.SaveChangesAsync();

                    // need to extend auction aswell
                    var auction = await _auctionRepository.GetOneAsync<Auction>(x => x.Id == record.AuctionId);
                    auction.ValidTo = extendTo;
                    response.Entity.RecordValidTo = extendTo;

                    await _auctionRepository.UpdateAsync(auction);
                }
                else
                {
                    response.Entity.RecordValidTo = record.ValidTo;
                }

                response.Status = MessageStatusEnum.Success; ;
                response.Code = "createdSuccessfully";
            }
            catch (Exception)
            {
                response.Status = MessageStatusEnum.Error;
                response.Code = "bidBadRequest";
            }

            return Json(response);
        }

        [Authorize("admin")]
        [HttpDelete("{id}")]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
            => Json(await _repository.RemoveAsync(id));

        [Authorize("read:messages")]
        [HttpGet("{recordId}")]
        [Route("sendEmailToOverbided")]
        public async Task<IActionResult> SendEmailToOverbidedCustomer(int recordId)
        {
            try
            {
                var record = await _recordRepository
                        .GetOneAsync<RecordTableDto>(x => x.Id == recordId);

                var userOverbidedIds = await _repository.Context.Bids
                    .Include(x => x.User)
                    .Include(x => x.User.Customer)
                    .Where(x => x.RecordId == recordId)
                    .OrderByDescending(x => x.Price)
                    .Take(2)
                    .Select(x => x.User.Id)
                    .ToListAsync();

                var overbidedUserId = userOverbidedIds.Last();
                var overbidedUser = await _userRepository.GetOneAsync<User>(x => x.Id == overbidedUserId);

                // await _userEmailFactory.SendAuctionOverbidenEmail(overbidedUser, record);

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}