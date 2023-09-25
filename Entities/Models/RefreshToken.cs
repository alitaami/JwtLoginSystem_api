using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class RefreshToken
    {
        // Unique ID for the refresh token entry
        [Key]
        public int Id { get; set; }

        // Actual Refresh Token value
        public string Token { get; set; }

        // Reference to the user that owns this refresh token
        public int UserId { get; set; } 

        // When the token was issued
        public DateTime IssuedAt { get; set; }

        // When the token expires and can no longer be used to get a new access token
        public DateTime ExpiresAt { get; set; }

        // Flag to mark the token as used, to ensure one-time use
        public bool IsUsed { get; set; }

        // Flag to mark the token as revoked, in case it was manually invalidated
        public bool IsRevoked { get; set; }

        // A method to determine if the token is still active
        public bool IsActive => !IsUsed && !IsRevoked && DateTime.UtcNow <= ExpiresAt;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }}

}
