using Microsoft.AspNetCore.Mvc;
using SA.Application.Account;
using SA.Application.Security;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        private readonly ISecurityService _securityService;

        public AccountsController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginForm)
            => Json(await _securityService.Login(loginForm));
    }
}