using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Alerts
{
    public class IndexViewModel
    {
         public IndexViewModel()
        {
        }

        public IndexViewModel(IEnumerable<BeautySNS.Domain.Model.Alert> alerts)
        {
            Alerts = alerts;
        }

        public IEnumerable<BeautySNS.Domain.Model.Alert> Alerts { get; set; }
        public bool userSession { get; set; }
        public int accountID { get; set; }
        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public string permissionType { get; set; }
        public bool adminUser { get; set; }
    }

    }
