using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.SocialLogins.Models
{
    public class ProfileModel
    {
        [Required(ErrorMessage = "Have to supply a display name")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Have to supply an email")]

        public string Email { get; set; }
    }
}
