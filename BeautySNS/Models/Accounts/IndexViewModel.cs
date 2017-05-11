using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }

        public IndexViewModel(IEnumerable<BeautySNS.Domain.Model.Account> accounts)
        {
            Accounts = accounts;
        }

        public IEnumerable<BeautySNS.Domain.Model.Account> Accounts { get; set; }
        
        public bool userSession { get; set; }
        public int accountID { get; set; }
        public string fullName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public int userAccountID { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public virtual Profile Profile { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
        //public bool superAdminUser { get; set; }

    }
}