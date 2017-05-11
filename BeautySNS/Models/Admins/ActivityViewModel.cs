using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Admins
{
    public class ActivityViewModel
    {
        public ActivityViewModel() { }

        public ActivityViewModel(Account account)
        {

        }

        public ActivityViewModel(AccountPermission accountPermission)
        {
            Account = accountPermission.Account;
            firstName = accountPermission.Account.firstName;
            lastName = accountPermission.Account.lastName;
            dateAdded = accountPermission.createDate;
            email = accountPermission.email;
            accountPermissionID = accountPermission.accountPermissionID;
        }

        public int accountID { get; set; }
        public int accountPermissionID { get; set; }
        public bool userSession { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public string permissionType { get; set; }
        public string userPermissionType { get; set; }
        public virtual Account Account { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime? dateAdded { get; set; }
        public string email { get; set; }
        public bool adminUser { get; set; }

    }
}