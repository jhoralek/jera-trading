using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Application.Country;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Countries")]
    public class CountriesController : BaseController<Country>
    {
        public CountriesController(
            IEntityRepository<Country> repository,
            IMapper mapper) :
            base(repository, mapper)
        { }

        [Authorize("read:messages")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _repository.GetAllAsync<CountryDto, string>(orderAsc: x => x.Name);
            return Json(countries);
        }
    }
}