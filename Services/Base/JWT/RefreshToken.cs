using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Base.JWT
{
    public class refreshToken
    {  
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; } 
        public DateTime IssuedAt { get; set; } 
        public DateTime ExpiresAt { get; set; } 
        public bool IsUsed { get; set; } 
        public bool IsRevoked { get; set; }
        public bool IsActive => !IsUsed && !IsRevoked && DateTime.UtcNow <= ExpiresAt;
    }
}
