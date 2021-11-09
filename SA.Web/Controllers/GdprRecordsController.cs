using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Application.Records;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/GdprRecords")]
    public class GdprRecordsController : BaseController<GdprRecord>
    {
        public GdprRecordsController(
            IEntityRepository<GdprRecord> repository, IMapper mapper)
            : base(repository, mapper) { }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] GdprRecord gdprRecord)
            => Json(await _repository.AddAsync(gdprRecord));

        [HttpGet]
        [Authorize("admin")]
        [Route("getAllGdprRecordsAdmin")]
        public async Task<IActionResult> GetAllGdprRecordsAdmin()
            => Json(await _repository
                .GetAllAsync<GdprRecordTableDto, DateTime?>(orderAsc: x => x.Created));

        [HttpGet]
        [Authorize("admin")]
        [Route("getById")]
        public async Task<IActionResult> GetById(int id)
            => Json(await _repository.GetOneAsync<GdprRecordDto>(x => x.Id == id));
    }
}