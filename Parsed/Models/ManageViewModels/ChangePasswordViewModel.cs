ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "OldPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "NewPasswordLength")]
        [DataType(DataType.Password, ErrorMessage = "NewPasswordTypeValidation")]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("NewPassword", ErrorMessage = "ConfirmPasswordCompare")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
