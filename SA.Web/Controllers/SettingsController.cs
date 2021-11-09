using Microsoft.AspNetCore.Mvc;
using SA.Application.Country;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Settings")]
    public class SettingsController : Controller
    {
        private readonly IEntityRepository<Country> _countryRepositiory;

        public SettingsController(
            IEntityRepository<Country> countryRepository)
        {
            _countryRepositiory = countryRepository;
        }

        [HttpGet("{lang}")]
        [Route("Countries")]
        public async Task<IActionResult> Countries(string lang)
        {
            var dict = new Dictionary<string, IEnumerable<CountryDto>>();
            var languages = await _countryRepositiory
                .GetAllAsync<CountryDto, string>(x =>
                    x.Language == lang,
                    x => x.Name);

            dict.Add(lang, languages);

            return Json(dict);
        }
    }
}
