using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
    public class RecoverPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter an email address")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string email { get; set; }

        public bool userSession { get; set; }

    }
}