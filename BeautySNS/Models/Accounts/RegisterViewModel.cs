using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
        public class RegisterViewModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string firstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string lastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string password { get; set; }

            [Required(ErrorMessage = "Please enter a date")]
            [DisplayName("Date of birth")]
            [DataType(DataType.Date)]
            public DateTime? birthDate { get; set; }

            public bool userSession { get; set; }
          
        }
    }
