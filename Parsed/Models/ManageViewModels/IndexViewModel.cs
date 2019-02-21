using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Required(ErrorMessage = "FirstNameRequired")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastNameRequired")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "IsEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "StatusMessage")]
        public string StatusMessage { get; set; }
    }
}
