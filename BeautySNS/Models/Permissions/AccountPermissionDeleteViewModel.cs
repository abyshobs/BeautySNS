using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Permissions
{
    public class AccountPermissionDeleteViewModel
    {
       public AccountPermissionDeleteViewModel() { }

        public AccountPermissionDeleteViewModel(AccountPermission accountPermission) { }

        public AccountPermissionDeleteViewModel(AccountPermission accountPermission, IEnumerable<Permission> permissions)
        {
            Permission = accountPermission.Permission;
            permissionID = accountPermission.permissionID;
            accountPermissionID = accountPermission.accountPermissionID;
            accountID = accountPermission.accountID;
            email = accountPermission.email;
            createDate = accountPermission.createDate;
            lastUpdateDate = accountPermission.lastUpdateDate;

        }

        public int accountPermissionID { get; set; }
        [DisplayName("Permission")]
        public int permissionID { get; set; }
        
        public int accountID { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("Date Created")]
        public DateTime? createDate { get; set; }

        [DisplayName("Last update date")]
        public DateTime? lastUpdateDate { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }
        public virtual Permission Permission { get; set; }
        public bool userSession { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
    }
}