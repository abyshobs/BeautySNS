using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Permissions
{
    public class AccountPermissionDetailsViewModel
    {
        public AccountPermissionDetailsViewModel() { }

        public AccountPermissionDetailsViewModel(IEnumerable<Permission> permissions) { }

        public AccountPermissionDetailsViewModel(AccountPermission accountPermission)
        {
            Permission = accountPermission.Permission;
            accountPermissionID = accountPermission.accountPermissionID;
            permissionID = accountPermission.permissionID;
            accountID = accountPermission.accountID;
            createDate = accountPermission.createDate;
            lastUpdateDate = accountPermission.lastUpdateDate;
            email = accountPermission.email;
           
        }

        public int accountPermissionID { get; set; }
        public int accountID { get; set; }

        [DisplayName("Permission")]
        public int permissionID { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Date Created")]
        public DateTime? createDate { get; set; }

        [DisplayName("Last Update Date")]
        public DateTime? lastUpdateDate { get; set; }

        public Permission Permission { get; set; }
        public bool userSession { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
    }
}