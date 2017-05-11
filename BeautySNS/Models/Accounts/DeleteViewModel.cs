using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
    public class DeleteViewModel
    {

        public DeleteViewModel() { }

        public DeleteViewModel(Account account)
        {
            accountID = account.accountID;
            firstName = account.firstName;
            lastName = account.lastName;
            birthDate = account.birthDate;
            email = account.email;
            dateCreated = account.dateCreated;
            dateUpdated = account.dateUpdated;
        }

        public int accountID { get; set; }

        [DisplayName("First Name")]
        public string firstName { get; set; }

        [DisplayName("Last Name")]
        public string lastName { get; set; }

        [DisplayName("D.O.B")]
        public DateTime? birthDate { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Date Created")]
        public DateTime? dateCreated { get; set; }

        [DisplayName("Last Update date")]
        public DateTime? dateUpdated { get; set; }

        public bool userSession { get; set; }

        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
    }
}