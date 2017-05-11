using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class Friend
    {
        public int friendID { get; set; }
        public int accountID { get; set; }
        public int myFriendsAccountID { get; set; }
        public DateTime? createDate { get; set; }

        public virtual Account Account { get; set; }
    }
}
