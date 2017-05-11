using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class Account
    {
        [Key]
        public int accountID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool emailVerified { get; set; }
        public DateTime? birthDate { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? dateUpdated { get; set; }

        public virtual Profile Profile { get; set; }
        public ICollection<Alert> Alerts { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<FriendInvitation> FriendInvitations { get; set; }
        public ICollection<AccountPermission> AccountPermissions { get; set; }
        public ICollection<StatusUpdate> StatusUpdates { get; set; }


    }
}
