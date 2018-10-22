ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Parsed.Models.ManageViewModels
{
    public class EnableAuthenticatorViewModel
    {
            [Required(ErrorMessage = "CodeRequired")]
            [StringLength(7, ErrorMessage = "CodeLength", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Code")]
            public string Code { get; set; }

            [BindNever]
            public string SharedKey { get; set; }

            [BindNever]
            public string AuthenticatorUri { get; set; }
    }
}
