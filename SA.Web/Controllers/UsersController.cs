using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SA.Application.Account;
using SA.Application.Email;
using SA.Application.Records;
using SA.Application.Security;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SA.WebApi.Controllers
{
    [Route("api/Users")]
    public class UsersController : BaseController<User>
    {
        private readonly ISecurityService _securityService;
        private readonly IEntityRepository<Customer> _customerRepository;
        private readonly IEntityRepository<Record> _recordRepository;
        private readonly IEntityRepository<UserActivation> _userActivationRepository;
        private readonly IUserEmailFactory _userEmailFactory;
        private readonly IConfiguration _configuration;

        public UsersController(
            IEntityRepository<User> repository,
            ISecurityService securityService,
            IEntityRepository<Customer> customerRepository,
            IEntityRepository<Record> recordRepository,
            IEntityRepository<UserActivation> userActivationRepository,
            IUserEmailFactory userEmailFactory,
            IConfiguration configuration,
            IMapper mapper)
            : base(repository, mapper)
        {
            _securityService = securityService;
            _customerRepository = customerRepository;
            _recordRepository = recordRepository;
            _userActivationRepository = userActivationRepository;
            _userEmailFactory = userEmailFactory;
            _configuration = configuration;
        }

        [Authorize("read:messages")]
        [Route("LoadByNameAndToken")]
        [HttpGet("{uName}/{token}")]
        public async Task<IActionResult> LoadByNameAndToken(string uName, string token)
            => Json(await _repository.GetOneAsync<User>(x => x.UserName == uName
                && x.Token == token));

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            user.Password = _securityService.GetMD5HashData(user.Password);
            user.Language = user.Customer.Address.Country.Language;
            user.Customer.Address.Country = null;

            var newUser = await _repository.AddAsync(user);

            if (newUser.Id > 0)
            {
                var token = Guid.NewGuid().ToString();
                var activation = await _userActivationRepository.AddAsync(new UserActivation
                {
                    IsUsed = false,
                    UserId = newUser.Id,
                    Token = token,
                });

                if (activation.Id > 0)
                {
                    await _userEmailFactory.SendActivationEmail(user, activation);
                }
            }

            return Redirect($"{_configuration["Web:Url"]}/successfully-registered");
        }

        [HttpGet]
        [Authorize("admin")]
        [Route("getAllUsersForAdmin")]
        public async Task<IActionResult> GetAllUsersForAdmin()
            => Json(await _repository
                .GetAllAsync<UserSimpleDto, DateTime>(orderDesc: u => u.Created));

        [HttpGet("{userId}")]
        [Authorize("read:messages")]
        [Route("getUsersCurrentAuctions")]
        public async Task<IActionResult> GetUsersCurrentAuctions(int userId)
        {
            var now = DateTime.Now;
            var records = await _recordRepository
                .GetAllAsync<RecordTableDto, DateTime>(x => x.IsActive
                    && x.ValidFrom <= now
                    && x.ValidTo >= now
                    && x.Bids.Any(y => y.UserId == userId), x => x.ValidFrom);

            return Json(records);
        }

        [HttpGet("{userId}")]
        [Authorize("read:messages")]
        [Route("getUsersEndedAuctions")]
        public async Task<IActionResult> GetUsersEndedAuctions(int userId)
        {
            var now = DateTime.Now;
            var records = await _recordRepository
                .GetAllAsync<RecordTableDto, DateTime>(x => x.ValidTo < now
                    && x.Bids.Any(y => y.UserId == userId), x => x.ValidFrom);

            return Json(records);
        }

        [HttpPost]
        [Authorize("admin")]
        [Route("sendMultipleEmail")]
        public async Task<IActionResult> SendMultipleEmail([FromBody] EmailMessage email)
        {
            if (!email.ToAddresses.Any())
            {
                return Json(false);
            }

            email.ToAddresses.ForEach(x =>
            {
                _userEmailFactory.SendEmail(new EmailMessage
                {
                    Subject = email.Subject,
                    Content = email.Content,
                    ToAddresses = new List<EmailAddress> { x },
                    FromAddresses = new List<EmailAddress>()
                });
            });

            return Json(true);
        }

        [HttpPost]
        [Route("updateUserAdmin")]
        [Authorize("admin")]
        public async Task<IActionResult> UpdateUserAdmin([FromBody] UserSimpleDto user)
        {
            var pUser = await _repository.GetOneAsync<User>(x => x.Id == user.Id);
            var pCustomer = await _customerRepository.GetOneAsync<Customer>(x => x.Id == pUser.CustomerId);
            pUser.IsActive = user.IsActive;
            pCustomer.IsFeePayed = user.IsFeePayed;

            await _repository.UpdateAsync(pUser);
            await _customerRepository.UpdateAsync(pCustomer);
            return Json(true);
        }

        [HttpPut]
        [Authorize("read:messages")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null && user.Id <= 0)
            {
                return BadRequest();
            }
            return Json(await _repository.UpdateAsync(user));
        }

        [HttpGet("{email}")]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _repository
                .GetOneAsync<UserSimpleDto>(x => x.Customer.Email == email);

            if (user == null) return Json(false);

            var token = Guid.NewGuid().ToString();
            var activation = await _userActivationRepository.AddAsync(new UserActivation
            {
                IsUsed = false,
                UserId = user.Id,
                Token = token,
            });

            await _userEmailFactory.ResetPassword(user, activation);

            return Json(true);
        }

        [HttpPost()]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPasswrd([FromBody] ChangePasswordDto model)
            => Json(await _securityService.ResetPassword(model));

        [HttpGet("{userName}")]
        [Route("checkUserName")]
        public async Task<IActionResult> CheckUserName(string userName)
            => Json(await _securityService.CheckUniqueUserName(userName));

        [HttpGet("{email}")]
        [Route("checkEmail")]
        public async Task<IActionResult> CheckEmail(string email)
            => Json(await _securityService.CheckUniqueEmailAddress(email));

        [Authorize("admin")]
        [HttpGet("{userId}")]
        [Route("getCustomerInfo")]
        public async Task<IActionResult> GetCustomerInfo(int userId)
            => Json(await _repository.GetOneAsync<UserShortDto>(x => x.Id == userId));
    }
}