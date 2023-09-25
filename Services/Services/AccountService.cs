using Common.Resources;
using Common.Utilities;
using Entities.Base;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Extensions.Logging;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : ServiceBase<AccountService>, IAccountService
    {
        private IJwtService _jwtService;
        public AccountService(IJwtService jwtService,ILogger<AccountService> logger) : base(logger)
        {
            _jwtService = jwtService;
        }

        public async Task<ServiceResult> Login(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                // TODO : if you have time move it to DB (using Docker Compose)
                var users = new List<User>()
                         {
                             new User()
                             {
                                 Id = 1,
                                 UserName = "admin",
                                 FullName = "admin",
                                 Email = "alitaami81@gmail.com",
                                 PasswordHash = "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=",
                                 Age = 30,
                             },
                             new User()
                             {
                                 Id = 2,
                                 UserName = "ali",
                                 FullName= "ali taami",
                                 Email = "alitaamicr7@gmail.com",
                                 PasswordHash = "8jr0nptvznD5VS2WniCx5y6jYyQOSw1ZpfsulA8c/3A=",
                                 Age = 21,
                             }
                         };

                if (tokenRequest.grant_type.Equals("refresh_token", StringComparison.OrdinalIgnoreCase))
                {

                }
                    if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(ErrorCodeEnum.BadRequest, "OAuth flow should be password !!", null);

                // Check user credentials

                var passwordHash = SecurityHelper.GetSha256Hash(tokenRequest.password);

                var result = users
                    .Where(p => p.UserName == tokenRequest.username && p.PasswordHash == passwordHash && p.IsActive)
                    .FirstOrDefault();

                if (result is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                if (!result.IsActive)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.UserIsNotActive, null);///

                // Generate JWT token

                var token = await _jwtService.Generate(result);

                return Ok(token);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);

            }
        }

        //public Task<ServiceResult> UserSignUp(UserViewModel user, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
