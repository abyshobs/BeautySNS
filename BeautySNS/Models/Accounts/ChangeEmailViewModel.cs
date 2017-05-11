using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
    public class ChangeEmailViewModel
    {
            [Required(ErrorMessage = "Please enter an email address")]
            [Display(Name = "New Email")]
            [EmailAddress]
            public string email { get; set; }

            public int accountID { get; set; }
            public bool userSession { get; set; }
            public Account loggedInAccount { get; set; }
            public int loggedInAccountID { get; set; }
            public bool adminUser { get; set; }
       
        }
    }
