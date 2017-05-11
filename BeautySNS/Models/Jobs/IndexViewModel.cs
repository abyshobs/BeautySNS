using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Jobs
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }

        public IndexViewModel(IEnumerable<BeautySNS.Domain.Model.Job> jobs)
        {
            Jobs = jobs;
        }

        public IEnumerable<BeautySNS.Domain.Model.Job> Jobs { get; set; }

        public bool userSession { get; set; }
        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public int accountID { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
     }
}