using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Pages
{
    public class PageViewModel
    {
        public bool adminUser { get; set; }
        public bool userSession { get; set; }
        public int accountID { get; set; }
        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
    }
}