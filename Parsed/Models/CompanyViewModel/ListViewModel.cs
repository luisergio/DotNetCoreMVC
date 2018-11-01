using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models.CompanyViewModel
{
    public class ListViewModel
    {
        public long ID { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public string CNPJ { get; set; }

        [Display(Name = "DigitalCertificate")]
        public byte[] DigitalCertificate { get; set; }

        public string DigitalCertificateFileName { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreatedByID { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Users")]
        public virtual IList<CompanyUser> Users { get; set; }
    }
}
