using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class FriendInvitation
    {
        public int friendInvitationID { get; set; }
        public int accountID { get; set; }
        public int becameAccountID { get; set; }
        public Guid GUID { get; set; }
        public DateTime? createDate { get; set; }
        public string email { get; set; }

        public virtual Account Account { get; set; }
    }
}
