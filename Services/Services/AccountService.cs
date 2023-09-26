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
        private const string GrantTypeRefreshToken = "refresh_token";

        public async Task<ServiceResult> Login(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                switch (tokenRequest.grant_type.ToLowerInvariant())
                {
                    case GrantTypePassword:
                        return await HandlePasswordGrantType(tokenRequest, cancellationToken);

                    case GrantTypeRefreshToken:
                        return await HandleRefreshTokenGrantType(tokenRequest, cancellationToken);

                    default:
                        return BadRequest(ErrorCodeEnum.BadRequest, "OAuth flow should be password or refresh_token !!", null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        private async Task<ServiceResult> HandlePasswordGrantType(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                var passwordHash = SecurityHelper.GetSha256Hash(tokenRequest.password); // Consider using bcrypt or argon2
                var user = await _user.GetUserByData(tokenRequest.username, passwordHash, cancellationToken);

                if (user == null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);

                if (!user.IsActive)
                    return BadRequest(ErrorCodeEnum.BadRequest, Resource.UserIsNotActive, null);

                var token = await _jwtService.GenerateAccessToken(user);
                // TODO: Also generate and return a refresh token here if needed.
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        private async Task<ServiceResult> HandleRefreshTokenGrantType(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the stored refresh token
                var storedRefreshTokenResult = await _user.GetRefreshTokenForUser(tokenRequest.username, cancellationToken);
                var storedRefreshToken = storedRefreshTokenResult.Data as RefreshToken;
  
                if (storedRefreshToken == null || storedRefreshToken.ExpiresAt < DateTime.UtcNow || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
                {
                    if (storedRefreshToken?.ExpiresAt < DateTime.UtcNow)
                    {
                        return await PrepareExpiredTokenResponse(tokenRequest, cancellationToken);
                    }
                    return await PrepareInvalidTokenResponse(tokenRequest, cancellationToken);
                }

                // Fetch associated user
                var userResult = await _user.GetUserByUsername(tokenRequest.username, cancellationToken);
                var user = userResult.Data as User;

                if (user == null || !user.IsActive)
                {
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);
                }

                // Mark current refresh token as used
                storedRefreshToken.IsUsed = true;
                await _repoR.UpdateAsync(storedRefreshToken, cancellationToken);
                  
                return Ok(storedRefreshToken.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during refresh token processing.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }
        private async Task<ServiceResult> PrepareInvalidTokenResponse(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                await _user.UpdateRefreshTokenForUser(tokenRequest.username, cancellationToken);
                return BadRequest(ErrorCodeEnum.BadRequest, "Invalid refresh token", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login process.");
                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        private async Task<ServiceResult> PrepareExpiredTokenResponse(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            try
            {
                await _user.UpdateRefreshTokenForUser(tokenRequest.username, cancellationToken);
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
