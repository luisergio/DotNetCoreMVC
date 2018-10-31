using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Parsed.Models
{
    public class Company
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "TitleRequired")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "CNPJRequired")]
        public string CNPJ { get; set; }

        [Display(Name = "DigitalCertificate")]
        public byte[] DigitalCertificate { get; set; }

        [Display(Name = "Users")]
        public virtual IList<CompanyUser> Users { get; set; }
    }
}
