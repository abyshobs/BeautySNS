using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class StatusUpdate
    {
        public int statusUpdateID { get; set; }
        public int accountID { get; set; }
        public string status { get; set; }
        public byte[] attachment { get; set; }
        public DateTime? createDate { get; set; }

        public virtual Account Account { get; set; }
    }
}
