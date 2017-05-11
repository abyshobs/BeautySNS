using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Permissions
{
    public class AccountPermissionCreateViewModel
    {
        public AccountPermissionCreateViewModel(){}

        public AccountPermissionCreateViewModel(IEnumerable<Permission> permissions)
        {
          Permissions = permissions;
        }

        public IEnumerable<Permission> Permissions {get; set;}
        
        public int accountPermissionID { get; set; }
        
        public int accountID { get; set; }

        [Required(ErrorMessage = "Please provide a Permission")]
        [DisplayName("Permission")]
        public int permissionID { get; set; }

        [Required(ErrorMessage = "Please provide an Email")]
        [DisplayName("Email")]
        [EmailAddress]
        public string email { get; set; }

        public bool userSession { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }

    }
}