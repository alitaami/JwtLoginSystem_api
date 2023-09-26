using Entities.Base;
using Entities.Models;
using Services.Base.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ServiceResult> GetUserByUsername(string userName, CancellationToken cancellationToken);
        public  Task<ServiceResult> GetRefreshTokenForUser(string userName, CancellationToken cancellationToken);
        public  Task<ServiceResult> UpdateRefreshTokenForUser(string username , CancellationToken cancellation);
        public Task<RefreshToken> GetRefreshTokenByToken(string token, CancellationToken cancellationToken);
        public Task<User> GetUserByData(string username, string password,CancellationToken cancellationToken);
         
    }
}
