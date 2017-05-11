using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Permissions
{
    public class AccountPermissionIndexViewModel
    {
        public AccountPermissionIndexViewModel()
        {
        }

        public AccountPermissionIndexViewModel(IEnumerable<BeautySNS.Domain.Model.AccountPermission> accountPermissions)
        {
            AccountPermissions = accountPermissions;
        }

        public IEnumerable<BeautySNS.Domain.Model.AccountPermission> AccountPermissions { get; set; }

        public bool userSession { get; set; }
        public int accountID { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
    }
}