using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class AlertType
    {
        public int alertTypeID { get; set; }
        public string name { get; set; }

        public enum AlertTypes
        {
            AccountCreated = 1,
            AccountModified = 2,
            ProfileCreated = 3,
            ProfileModified = 4,
            StatusUpdate = 5,
            PermissionCreated = 6,
            AdminCreated = 7,
            JobCreated = 8,
            JobUpdated = 9,
            AdminUpdated = 10,
            AdminRemoved = 11,
            AccountRemoved = 12,
            StatusUpdateRemoved = 13
            //NewAvatar = 6,
            //AddedFriend = 7,
            //AddedPicture = 8,
            //FriendAdded = 9,
            //FriendRequest = 10,
            
        }

        public ICollection<Alert> Alerts { get; set; }
    }
}
