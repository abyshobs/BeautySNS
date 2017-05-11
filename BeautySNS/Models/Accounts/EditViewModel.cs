using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
    public class EditViewModel
    {
        public EditViewModel() { }

        public EditViewModel(Account account)
        {
            accountID = account.accountID;
            firstName = account.firstName;
            lastName = account.lastName;
            birthDate = account.birthDate;
        }

        public int accountID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [DisplayName("D.O.B")]
        public DateTime? birthDate { get; set; }

        public bool userSession { get; set; }
        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public bool adminUser { get; set; }
    }
}