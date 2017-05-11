using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Jobs
{
    public class CreateViewModel
    {
        public int jobID { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please provide a Name")]
        public string name {get; set;}

        public bool userSession { get; set; }
        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public string permissionType { get; set; }
        public int accountID { get; set; }
        public bool adminUser { get; set; }
    }
}