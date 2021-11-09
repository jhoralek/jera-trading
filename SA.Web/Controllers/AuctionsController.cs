using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA.Application.Records;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using SA.WebApi.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sa.WebApi.Controllers
{
    [Route("api/Auctions")]
    public class AuctionsController : BaseController<Auction>
    {
        public AuctionsController(IEntityRepository<Auction> repository, IMapper mapper)
            : base(repository, mapper) { }

        [HttpGet]
        [Route("getAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            var now = DateTime.Now;
            return Json(await _repository
                .GetAllAsync<AuctionDto, DateTime>(x =>
                    x.IsActive &&
                    x.ValidFrom <= now &&
                    x.ValidTo >= now,
                    x => x.ValidFrom));
        }

        [HttpGet]
        [Route("getAllInFeature")]
        public async Task<IActionResult> GetAllInFeature()
        {
            var now = DateTime.Now;
            var auctions = await _repository
                .GetAllAsync<AuctionDto, DateTime>(x =>
                    x.IsActive &&
                    x.ValidFrom > now,
                    x => x.ValidFrom);

            return Json(auctions);
        }
        [Authorize("admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Auction auction)
        {
            var persistedItem = await _repository.GetOneAsync<Auction>(x => x.Id == auction.Id);
            if (auction == null && persistedItem == null)
            {
                return BadRequest();
            }
            return Json(await _repository.UpdateAsync(auction));
        }

        [Authorize("admin")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Auction auction)
            => Json(await _repository.AddAsync(auction));

        [Authorize("admin")]
        [HttpGet]
        [Route("getAllForAdmin")]
        public async Task<IActionResult> GetAllForAdmin()
            => Json(await _mapper.ProjectTo<AuctionTableDto>(
                _repository.Context.Auctions
                    .Include(x => x.Records)
                    .OrderByDescending(x => x.IsActive)
                    .ThenByDescending(x => x.ValidFrom)
                    .ThenByDescending(x => x.ValidTo))
                .ToListAsync());


        [Authorize("admin")]
        [HttpDelete("{id}")]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
            => Json(await _repository.RemoveAsync(id));

        [Route("getForEdit")]
        [Authorize("admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForEdit(int id)
            => Json(await _repository
                .GetOneAsync<AuctionDto>(x => x.Id == id));

        [Route("getAllActiveLookup")]
        [Authorize("admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllActiveLookup()
            => Json(await _repository
                .GetAllAsync<AuctionLookupDto, DateTime>(x =>
                    x.IsActive,
                    x => x.ValidFrom));

        [Route("getAllLookup")]
        [Authorize("admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllLookup()
            => Json(await _mapper
                .ProjectTo<AuctionLookupDto>(_repository.Context.Auctions
                    .Include(x => x.Records)
                    .OrderByDescending(x => x.IsActive)
                    .ThenByDescending(x => x.ValidFrom)
                    .ThenByDescending(x => x.ValidTo))
                .ToListAsync());
    }
}
