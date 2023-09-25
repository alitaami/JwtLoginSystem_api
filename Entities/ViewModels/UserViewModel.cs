using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class UserViewModel
    {

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "That should be in email format!")]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(500)]
        public string Password { get; set; }
        [Required]
        [StringLength(500)]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
