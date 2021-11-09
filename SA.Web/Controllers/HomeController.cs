using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SA.Web.Models;
using System.Diagnostics;

namespace SA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewData["ApiUrl"] = _configuration["ApiUrl:Url"];
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });

    }
}
