using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models.AccountViewModels
{
    public class LoginWithRecoveryCodeViewModel
    {
            [Required(ErrorMessage = "RecoveryCodeRequired")]
            [DataType(DataType.Text)]
            [Display(Name = "RecoveryCode")]
            public string RecoveryCode { get; set; }
    }
}
