using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class JwtToken
    {
        [Required]
        [StringLength(200)]
        public string Token { get; set; }
    }
}
