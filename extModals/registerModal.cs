using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RealEstate.Models;

namespace RealEstate.extModals
{
    public class registerModal
    {
        [Required]
        [Display(Name = "Full Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string UserPhone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "The email address is required")]
        //[DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        //[DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Compare("UserEmail", ErrorMessage = "The Email and confirmation email do not match.")]
        public string confirmEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "The City Name is required")]
        public city cities { get; set; }
    }
}