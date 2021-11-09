using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/UserActivations")]
    public class UserActivationsController : Controller
    {
        private readonly IEntityRepository<UserActivation> _userActivationsRepositiory;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public UserActivationsController(
            IEntityRepository<UserActivation> userActivationsRepositiory,
            IConfiguration configuration,
            IEntityRepository<User> userRepository)
        {
            _configuration = configuration;
            _userActivationsRepositiory = userActivationsRepositiory;
            _userRepository = userRepository;
        }

        [HttpGet("{token}")]
        [Route("userValidation")]
        public async Task<IActionResult> UserValidation(string token)
        {
            var record = await _userActivationsRepositiory
                .GetOneAsync<UserActivation>(x =>
                    x.Token == token &&
                    !x.IsUsed);

            if (record == null) return Redirect($"{_configuration["Web:Url"]}/validation-expired");

            record.IsUsed = true;
            await _userActivationsRepositiory.UpdateAsync(record);

            var user = await _userRepository.GetOneAsync<User>(x => x.Id == record.UserId);

            user.IsActive = true;
            await _userRepository.UpdateAsync(user);

            return Redirect($"{_configuration["Web:Url"]}/validated-successfully?token={token}");
        }

        [HttpGet("{token}")]
        [Route("userPasswordReset")]
        public async Task<IActionResult> UserPasswordReset(string token, string password)
        {
            var record = await _userActivationsRepositiory
                .GetOneAsync<UserActivation>(x =>
                    x.Token == token &&
                    !x.IsUsed);

            if (record == null) return Redirect($"{_configuration["Web:Url"]}/password-reset-failed");

            record.IsUsed = true;
            await _userActivationsRepositiory.UpdateAsync(record);

            return Redirect($"{_configuration["Web:Url"]}/password-reset?token={token}");
        }
    }
}
