using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Services.Base.JWT;

namespace Services.Services.Interfaces
{
    public interface IJwtService
    {
        Task<AccessToken> Generate(User user);
    }
}
