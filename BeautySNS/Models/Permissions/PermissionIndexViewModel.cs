using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Permissions
{
    public class PermissionIndexViewModel
    {
        public PermissionIndexViewModel()
        {
        }

        public PermissionIndexViewModel(IEnumerable<BeautySNS.Domain.Model.Permission> permissions)
        {
            Permissions = permissions;
        }

        public IEnumerable<BeautySNS.Domain.Model.Permission> Permissions { get; set; }
        public bool userSession { get; set; }
        public int accountID { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
    }
}

 
