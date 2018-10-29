using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Parsed.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName {
            get {
                return String.Concat(FirstName, " ", LastName);
            }
        }

        [NotMapped]
        public virtual IList<Company> Companies { get; set; }

        #endregion
    }
}
