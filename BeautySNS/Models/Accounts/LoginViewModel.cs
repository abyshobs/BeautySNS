using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
   
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter an email address")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        public bool userSession { get; set; }
       
    }
}