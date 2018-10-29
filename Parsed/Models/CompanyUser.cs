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
        public int CompanyID { get; set; }

        [ForeignKey("ApplicationUser")]
        public int UserID { get; set; }


        public CompanyUserRole Role { get; set; }
    }

    public enum CompanyUserRole
    {
        Administrator = 1,
        Viewer = 2
    }
}
