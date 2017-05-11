using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class Alert
    {
        public int alertID { get; set; }
        public int alertTypeID { get; set; }
        public int accountID { get; set; }
        public bool isHidden { get; set; }
        public string message { get; set; }
        public DateTime? createDate { get; set; }

        public virtual AlertType AlertType { get; set; }
        public virtual Account Account { get; set; }
    }
}
