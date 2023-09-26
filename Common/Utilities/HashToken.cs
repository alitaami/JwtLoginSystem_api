using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Common.Utilities.HashToken;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Common.Utilities
{

    public static class HashToken
    {
        public static string HashRefreshToken(string tokenValue)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute and return the hash
                return Convert.ToBase64String(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(tokenValue)));
            }
        }
    }
}
