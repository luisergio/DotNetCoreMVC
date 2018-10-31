using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parsed.Models
{
    public class CompanyUser
    {
        [ForeignKey("Company")]
        public long CompanyID { get; set; }

        public Company Company { get; set; }

        [ForeignKey("ApplicationUser")]
        [MaxLength(450)]
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        public CompanyUserRole Role { get; set; }
    }

    public enum CompanyUserRole
    {
        Administrator = 1,
        Viewer = 2
    }
}
