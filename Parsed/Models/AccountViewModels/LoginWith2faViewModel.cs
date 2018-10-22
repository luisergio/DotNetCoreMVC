ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required(ErrorMessage = "AuthenticatorCodeRequired")]
        [StringLength(7, ErrorMessage = "AuthenticatorCodeLength", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "AuthenticatorCode")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "RememberMachine")]
        public bool RememberMachine { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
