using Entities.Base;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<ServiceResult> Login(TokenRequest tokenRequest, CancellationToken cancellationToken);
        //public Task<ServiceResult> UserSignUp(UserViewModel user, CancellationToken cancellationToken);

    }
}
