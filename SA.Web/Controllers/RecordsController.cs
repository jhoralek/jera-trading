using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA.Application.Records;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Records")]
    public class RecordsController : BaseController<Record>
    {
        private IEntityRepository<Auction> _auctionRepo;
        public RecordsController(
                IEntityRepository<Record> repository,
                IEntityRepository<Auction> auctionRepo,
                IMapper mapper)
            : base(repository, mapper)
        {
            _auctionRepo = auctionRepo;
        }

        [HttpGet]
        [Route("getAllForList")]
        public async Task<IActionResult> GetAllForList()
            => Json(await _repository
                .GetAllAsync<RecordTableDto, string>(x =>
                    x.IsActive &&
                    x.ValidTo >= DateTime.Now,
                x => x.Name));

        [HttpGet]
        [Route("getAllCurrentAuctionItems")]
        public async Task<IActionResult> GetAllCurrentAuctionItems()
        {
            var today = DateTime.Now;
            var records = await _repository.GetAllAsync<RecordTableDto, DateTime>(x => x.IsActive
                && x.Auction.IsActive
                && x.Auction.ValidFrom <= today
                && x.Auction.ValidTo >= today
                && !x.Auction.IsEnded
                && x.ValidFrom <= today
                && x.ValidTo >= today,
                x => x.ValidTo);

            return Json(records);
        }

        [HttpGet]
        [Route("getAllCurrentAuctionItemsMinimum")]
        public async Task<IActionResult> GetAllCurrentAuctionItemsMinimum()
        {
            var today = DateTime.Now;
            var records = await _repository.GetAllAsync<RecordMinimumDto, DateTime>(x => x.IsActive
                && x.Auction.IsActive
                && x.Auction.ValidFrom <= today
                && x.Auction.ValidTo >= today
                && !x.Auction.IsEnded
                && x.ValidFrom <= today
                && x.ValidTo >= today,
                x => x.ValidTo);

            return Json(records);
        }

        [Route("getById")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetOneAsync<RecordDetailDto>(x => x.Id == id);

            if (item != null)
            {
                if (item.Bids.Any())
                {
                    item.Bids = item.Bids.OrderByDescending(x => x.Price).Take(3).ToList();
                }
            }

            return Json(item);
        }
        [Authorize("admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Record record)
        {
            var persistedItem = await _repository.GetOneAsync<Record>(x => x.Id == record.Id);
            if (record == null && persistedItem == null)
            {
                return BadRequest();
            }
            return Json(await _repository.UpdateAsync(record));
        }

        [Authorize("read:messages")]
        [HttpGet("{id}")]
        [Route("allActiveWithUsersBids")]
        public async Task<IActionResult> AllActiveWithUsersBids(int id)
        {
            var now = DateTime.Now;
            var result = await _repository.GetAllAsync<RecordMinimumDto, DateTime>(x =>
                    x.Bids.Any(y => y.UserId == id) &&
                    x.IsActive &&
                    x.ValidTo >= now,
               x => x.ValidTo);

            return Json(result);
        }

        [Authorize("admin")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Record record)
            => Json(await _repository.AddAsync(record));


        [Authorize("admin")]
        [HttpGet]
        [Route("getAllForAdmin")]
        public async Task<IActionResult> GetAllForAdmin()
            => Json(await _mapper.ProjectTo<RecordTableDto>(_repository.Context.Records
                    .Include(x => x.User)
                    .Include(x => x.Bids)
                    .Include(x => x.Files)
                    .OrderByDescending(x => x.Auction.IsActive)
                    .ThenByDescending(x => x.IsActive)
                    .ThenByDescending(x => x.ValidFrom)
                    .ThenByDescending(x => x.ValidTo))
                .ToListAsync());

        [Authorize("admin")]
        [HttpGet("{id}")]
        [Route("getAuctionRecordsForAdmin")]
        public async Task<IActionResult> GetAuctionRecordsForAdmin(int id)
            => Json(await _mapper.ProjectTo<RecordTableDto>(_repository.Context.Records
                    .Where(x => x.AuctionId == id)
                    .Include(x => x.User)
                    .Include(x => x.Bids)
                    .Include(x => x.Files)
                    .OrderByDescending(x => x.Auction.IsActive)
                    .ThenByDescending(x => x.IsActive)
                    .ThenByDescending(x => x.ValidFrom)
                    .ThenByDescending(x => x.ValidTo))
                .ToListAsync());          

        [Authorize("admin")]
        [HttpDelete("{id}")]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
            => Json(await _repository.RemoveAsync(id));

        [HttpGet]
        [Route("actualRandom")]
        public async Task<IActionResult> ActualRandom()
        {
            var today = DateTime.Now;

            var items = await _repository
                .GetAllAsync<RecordDetailDto, int>(
                    x =>
                        x.IsActive
                        && x.Auction.IsActive
                        && x.Auction.ValidFrom <= today
                        && x.Auction.ValidTo >= today
                        && !x.Auction.IsEnded
                        && x.ValidFrom <= today
                        && x.ValidTo >= today,
                    x => x.Id);

            var randomId = items.Select(x => x.Id).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            var item = items.FirstOrDefault(x => x.Id == randomId);

            if (item != null)
            {
                if (item.Bids.Any())
                {
                    item.Bids = item.Bids.OrderByDescending(x => x.Price).Take(3).ToList();
                }
            }

            return Json(item);
        }

        [HttpGet("{take}")]
        [Route("getLatestEndedRecords")]
        public async Task<IActionResult> GetLatestEndedRecords(int take = 20)
        {
            var now = DateTime.Now;
            var items = await _repository
                .GetAllAsync<RecordTableDto, DateTime>(x => x.ValidFrom < now
                    && x.ValidTo < now,
                    orderDesc: x => x.ValidTo,
                    take: take);

            return Json(items);
        }

        [HttpGet]
        [Authorize("admin")]
        [Route("getAllEndedRecords")]
        public async Task<IActionResult> GetAllEndedRecords()
        {
            var now = DateTime.Now;
            return Json(await _repository.GetAllAsync<RecordTableDto, int>(x => x.ValidFrom < now
                && x.ValidTo < now, x => x.Id));
        }

        [Authorize("admin")]
        [HttpGet("{id}")]
        [Route("updateRecordDatesByAuctionId")]
        public async Task<IActionResult> UpdateRecordDatesByAuctionId(int id)
        {
            var auction = await _auctionRepo.GetOneAsync<AuctionLookupDto>(x => x.Id == id);
            var items = await _repository.Context.Records
                    .Where(x => x.AuctionId == auction.Id)
                    .Include(x => x.User)
                    .Include(x => x.Bids)
                    .Include(x => x.Files)
                    .ToListAsync();

            foreach (var item in items)
            {
                item.ValidFrom = auction.ValidFrom;
                item.ValidTo = auction.ValidTo;

                await _repository.UpdateAsync(item);
            }

            return await GetAuctionRecordsForAdmin(id);
        }
    }
}