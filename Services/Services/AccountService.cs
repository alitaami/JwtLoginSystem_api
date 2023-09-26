using Common.Resources;
using Common.Utilities;
using Data.Repositories;
using Entities.Base;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Extensions.Logging;
using Services.Base.JWT;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Services
{
    public class AccountService : ServiceBase<AccountService>, IAccountService
    {
        private IJwtService _jwtService;
        private IUserService _user;
        private IRepository<RefreshToken> _repoR;
        public AccountService(IRepository<RefreshToken> repoR, IUserService user, IJwtService jwtService, ILogger<AccountService> logger) : base(logger)
        {
            _repoR = repoR;
            _user = user;
            _jwtService = jwtService;
        }


        private const string GrantTypePassword = "password"; 
        public async Task<ServiceResult> RegisterUser(UserViewModel request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Check if the user already exists
                var existingUser = await _user.GetUserByUsername(request.UserName, cancellationToken);

                if (existingUser != null)
                {
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.AlreadyExists, null);
                }

                // 2. Hash the password (bcrypt, argon2, etc.)
                var hashedPassword = SecurityHelper.GetSha256Hash(request.Password);

                // 3. Create a new user entity and save it
                var newUser = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    FullName = request.FullName,
                    Age = request.Age,
                    IsActive = true

                };
                await _user.AddUser(newUser, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration process.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        } 

        public async Task<ServiceResult> Login(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                if (!tokenRequest.grant_type.Equals(GrantTypePassword, StringComparison.OrdinalIgnoreCase))
                    return BadRequest(ErrorCodeEnum.BadRequest, "OAuth flow should be password !!", null);

                var passwordHash = SecurityHelper.GetSha256Hash(tokenRequest.password); // Consider using bcrypt or argon2
                var user = await _user.GetUserByData(tokenRequest.username, passwordHash, cancellationToken);

                if (user == null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

                if (!user.IsActive)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.UserIsNotActive, null);

                var accessToken = await _jwtService.GenerateAccessToken(user);

                // TODO: Save the refresh token in the database.
                var refreshTokenResult = await _user.UpdateRefreshTokenForUser(user.UserName, cancellationToken);
                var refreshToken = refreshTokenResult.Data as RefreshToken;

                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
     
        public async Task<ServiceResult> GetNewAccessTokenUsingRefreshToken(string refreshToken,string username, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the stored refresh token
                var storedRefreshToken = await _user.GetRefreshTokenByToken(refreshToken , cancellationToken);

                if (storedRefreshToken == null || storedRefreshToken.ExpiresAt < DateTime.UtcNow || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
                {
                    if (storedRefreshToken?.ExpiresAt < DateTime.UtcNow)
                    {
                        return await PrepareExpiredTokenResponse(username,cancellationToken); // Adjusted this to return a BadRequest directly.
                    }
                    return   await PrepareInvalidTokenResponse(username,cancellationToken); // Adjusted this to return a BadRequest directly.
                }

                // Fetch associated user
                var user = await _user.GetUserById(storedRefreshToken.UserId, cancellationToken);

                if (user == null || !user.IsActive)
                {
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);
                }

                // Generate a new access token for the user
                var newAccessToken = await _jwtService.GenerateAccessToken(user);

                // Mark current refresh token as used (Optional: Consider generating a new refresh token too and sending it along)
                storedRefreshToken.IsUsed = true;
                await _repoR.UpdateAsync(storedRefreshToken, cancellationToken);

                return Ok(new
                {
                    AccessToken = newAccessToken,
                    // Optionally, if you're generating a new refresh token: RefreshToken = newRefreshToken.Token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during refresh token processing.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

         private async Task<ServiceResult> PrepareInvalidTokenResponse(string username, CancellationToken cancellationToken)
        {
            try
            {
                await _user.UpdateRefreshTokenForUser(username, cancellationToken);
                return BadRequest(ErrorCodeEnum.BadRequest, "Invalid refresh token", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        private async Task<ServiceResult> PrepareExpiredTokenResponse(string username, CancellationToken cancellationToken)
        {
            try
            {
                await _user.UpdateRefreshTokenForUser( username, cancellationToken);
                return BadRequest(ErrorCodeEnum.BadRequest, Resource.ExpiredDate, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }


        //public Task<ServiceResult> UserSignUp(UserViewModel user, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
