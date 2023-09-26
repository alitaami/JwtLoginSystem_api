using Common.Resources;
using Common.Utilities;
using Data.Repositories;
using Entities.Base;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService : ServiceBase<UserService>, IUserService
    {
        private IRepository<User> _repo;
        private IRepository<RefreshToken> _repoR;
        private IJwtService _jwt;
        public UserService(IJwtService jwt, IRepository<RefreshToken> repoR, IRepository<User> repo, ILogger<UserService> logger) : base(logger)
        {
            _jwt = jwt;
            _repo = repo;
            _repoR = repoR;
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string token, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _repoR.TableNoTracking.Where(x => x.Token == token).FirstOrDefaultAsync(cancellationToken);

                if (res is null)
                    return null;///

                return res;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                throw new Exception(Resource.GeneralErrorTryAgain);
            }
        }

        public async Task<ServiceResult> GetRefreshTokenForUser(string userName, CancellationToken cancellationToken)
        {
            try
            {
                var userResult = await GetUserByUsername(userName, cancellationToken);
                var user = userResult.Data as User;

                if (user is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                var refreshToken = await _repoR.TableNoTracking.Where(x => x.UserId == user.Id).FirstOrDefaultAsync(cancellationToken);

                if (refreshToken is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                return Ok(refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }

        }

        public async Task<User> GetUserByData(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repo.TableNoTracking
                .Where(x => x.UserName == username && x.PasswordHash == password).FirstOrDefaultAsync(cancellationToken);

                if (user is null)
                    return null;///

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                throw new Exception(Resource.GeneralErrorTryAgain);
            }
        }

        public async Task<ServiceResult> GetUserByUsername(string userName, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _repo.TableNoTracking
                    .Where(x => x.UserName == userName)
                    .FirstOrDefaultAsync(cancellationToken);

                if (user is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            }
        }

        public async Task<ServiceResult> UpdateRefreshTokenForUser(string username, CancellationToken cancellation)
        {
            try
            {
                // Find the user by username
                var useResult = await GetUserByUsername(username, cancellation);
                var user = useResult.Data as User;

                if (user is null)
                    return NotFound(ErrorCodeEnum.NotFound, Resource.NotFound, null);///

                // Assuming the RefreshToken property on the user entity
                // if you have a separate table for RefreshTokens, then you'll need to query that table
                var refreshTokenEntity = await _repoR.TableNoTracking.SingleOrDefaultAsync(rt => rt.UserId == user.Id);

                var token = await _jwt.GenerateRefreshToken(user);

                if (refreshTokenEntity == null)
                {
                    // This user doesn't have a refresh token, so let's create one
                    await _repoR.AddAsync(new RefreshToken
                    { 
                        Token = HashToken.HashRefreshToken(token.Token),
                        UserId = token.UserId,
                        IssuedAt = token.IssuedAt,
                        ExpiresAt = token.ExpiresAt,  // Example expiration
                        IsUsed = false,
                        IsRevoked = false
                    }, cancellation);
                }
                else
                {
                    // Update the existing refresh token
                     
                    refreshTokenEntity.Token = HashToken.HashRefreshToken(token.Token);
                    refreshTokenEntity.IssuedAt = token.IssuedAt;
                    refreshTokenEntity.ExpiresAt = token.ExpiresAt; // Update expiration as per your needs
                    refreshTokenEntity.IsUsed = token.IsUsed;
                    refreshTokenEntity.IsRevoked = token.IsRevoked;

                    await _repoR.UpdateAsync(refreshTokenEntity, cancellation);
                }
                
                // send a data that includes exact Token (not hashed one)
                var result = new RefreshToken
                {
                    Token = token.Token,
                    UserId = token.Id,
                    IssuedAt = token.IssuedAt,
                    ExpiresAt = token.ExpiresAt,  // Example expiration
                    IsUsed = false,
                    IsRevoked = false
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);

                return InternalServerError(ErrorCodeEnum.InternalError, Resource.GeneralErrorTryAgain, null);
            } 
        }
    }
}
