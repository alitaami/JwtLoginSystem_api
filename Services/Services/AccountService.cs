using Entities.Base;
using Entities.ViewModels;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        public Task<ServiceResult> Login(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UserSignUp(UserViewModel user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
