using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "FirstNameRequired")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastNameRequired")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(100, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "PasswordTypeValidation")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "ConfirmPasswordCompare")]
        public string ConfirmPassword { get; set; }
    }
}
