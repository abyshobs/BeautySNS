using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class Profile
    {
        
        public int profileID { get; set; }
        [Key]
        [ForeignKey("Account")]
        public int accountID { get; set; }
        public int jobID { get; set; }
        public byte[] avatar { get; set; }
        public string avatarMIMEType { get; set; }
        public string aboutMe { get; set; }
        public string education { get; set; }
        public string experience { get; set; }
        public string website { get; set; }
        public string location { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? lastUpdateDate { get; set; }
               
        public virtual Job Job { get; set; }
        public virtual Account Account { get; set; }
    }
}

