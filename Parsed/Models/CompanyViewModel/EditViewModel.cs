using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Parsed.Models.CompanyViewModel
{
    public class EditViewModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "TitleRequired")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "CNPJRequired")]
        public string CNPJ { get; set; }

        [Display(Name = "DigitalCertificate")]
        public IFormFile DigitalCertificate { get; set; }

    }
}
