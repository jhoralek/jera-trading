﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SA.Application.Account;
using SA.Core.Model;
using SA.EntityFramework.EntityFramework.Repository;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SA.Application.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _configuration;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<Core.Model.Customer> _customerRepository;
        private readonly IEntityRepository<UserActivation> _userActivationRepository;

        public SecurityService(IConfiguration configuration,
            IEntityRepository<User> userRepository,
            IEntityRepository<Core.Model.Customer> customerRepository,
            IEntityRepository<UserActivation> userActivationRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _userActivationRepository = userActivationRepository;
        }

        public async Task<AuthResponse> Login(LoginUserDto user)
        {
            var hashPassword = GetMD5HashData(user.Password);
            try
            {
                var persistedUser = await _userRepository.GetOneAsync<User>(x =>
                    x.UserName == user.UserName &&
                    x.Password == hashPassword &&
                    x.IsActive);

                if (persistedUser == null)
                {
                    return new AuthResponse { Error = "UserNotAuthenticated" };
                }

                var token = await RequestToken();

                if (!string.IsNullOrWhiteSpace(token.Error))
                {
                    return new AuthResponse
                    {
                        Error = $"{token.Error}: {token.ErrorDescription}"
                    };
                }

                persistedUser.Token = $"{token.TokenType} {token.AccessToken}";
                await _userRepository.UpdateAsync(persistedUser);

                return new AuthResponse
                {
                    UserId = persistedUser.Id,
                    Token = persistedUser.Token,
                    UserName = persistedUser.UserName,
                    Language = persistedUser.Language,
                    IsDealer = persistedUser.Customer.IsDealer,
                    IsFeePayed = persistedUser.Customer.IsFeePayed,
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<TokenDto> RequestToken()
        {
            using (var client = new HttpClient())
            {
                var url = $"https://{_configuration["Auth0:Domain"]}/oauth/token";

                var json = new
                {
                    client_id = _configuration["Auth0:ClientId"],
                    client_secret = _configuration["Auth0:ClientSecret"],
                    audience = _configuration["Auth0:Audience"],
                    grant_type = _configuration["Auth0:GrandType"]
                };

                var param = new StringContent(
                        JsonConvert.SerializeObject(json),
                        Encoding.UTF8,
                        "application/json");

                using (var res = await client.PostAsync(url, param))
                {
                    using (var content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            return JsonConvert.DeserializeObject<TokenDto>(data);
                        }
                        return null;
                    }
                }
            }
        }

        public string GetMD5HashData(string data)
        {
            var md5 = MD5.Create();
            var hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));
            var returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }

        public async Task<AuthResponse> ResetPassword(ChangePasswordDto obj)
        {
            if (obj.NewPassword != obj.RepeatNewPassword)
                return new AuthResponse { Error = "PasswordsNotSame" };            

            var activation = await _userActivationRepository.GetOneAsync<UserActivation>(x => x.Token == obj.Token);

            if (activation == null)
                return new AuthResponse { Error = "ResetTokenExpired" };

            var user = await _userRepository.GetOneAsync<User>(x => x.Id == activation.UserId);

            if (user == null)
                return new AuthResponse { Error = "UserNotExists", UserId = activation.UserId };

            user.Password = GetMD5HashData(obj.NewPassword);
            await _userRepository.UpdateAsync(user);

            return await Login(new LoginUserDto { UserName = user.UserName, Password = obj.NewPassword });
        }

        public async Task<bool> CheckUniqueUserName(string userName)
            => !await _userRepository.Context.Users.AnyAsync(x => x.UserName == userName);

        public async Task<bool> CheckUniqueEmailAddress(string email)
            => !await _userRepository.Context.Customers.AnyAsync(x => x.Email == email);
    }
}
