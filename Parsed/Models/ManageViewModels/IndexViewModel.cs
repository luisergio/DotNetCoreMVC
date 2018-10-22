ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "IsEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailInvalid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "StatusMessage")]
        public string StatusMessage { get; set; }
    }
}
