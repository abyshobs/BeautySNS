using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Permissions
{
    public class AccountPermissionEditViewModel
    {
        public AccountPermissionEditViewModel() { }

        public AccountPermissionEditViewModel(AccountPermission accountPermission) { }

        public AccountPermissionEditViewModel(AccountPermission accountPermission, IEnumerable<Permission> permissions)
        {
            Permission = accountPermission.Permission;
            permissionID = accountPermission.permissionID;
            accountPermissionID = accountPermission.accountPermissionID;
            accountID = accountPermission.accountID;
            email = accountPermission.email;
        }

        public int accountPermissionID { get; set; }

        [DisplayName("Permission")]
        public int permissionID { get; set; }
       
        public int accountID { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }
   
        public IEnumerable<Permission> Permissions { get; set; }
        public virtual Permission Permission { get; set; }
        public bool userSession { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
    }
}