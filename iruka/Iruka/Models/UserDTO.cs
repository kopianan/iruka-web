using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Iruka.EF.Model.Enum;

namespace Iruka.Models
{
    public class UserDTO
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name*")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Role*")]
        public InternalRoleEnum InternalRoleEnum { get; set; }

        [Display(Name = "User Role*")]
        public EndClientEnum EndClientEnum { get; set; }
        public string Picture { get; set; }
        public string Base64URL { get; set; }
        public string Certificate { get; set; }
        public string Role { get; set; }
        public string Base64URLCertificate { get; set; }
    }
}