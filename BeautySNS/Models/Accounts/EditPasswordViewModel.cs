using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Accounts
{
    public class EditPasswordViewModel
    {
        public EditPasswordViewModel() { }
      
        public int accountID { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string currentPassword { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "New Password")]
        public string newPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("newPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string confirmNewPassword { get; set; }

        public bool userSession { get; set; }
        public bool adminUser { get; set; }
        public int loggedInAccountID { get; set; }
        public Account loggedInAccount { get; set; }
    }
}