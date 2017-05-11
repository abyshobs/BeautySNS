using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Friends
{
    public class PendingRequestsViewModel
    {
         public PendingRequestsViewModel()
         {
         }

         public PendingRequestsViewModel(FriendInvitation invitation)
         {
             friendInvitationID = invitation.friendInvitationID;
         }


         public PendingRequestsViewModel(IEnumerable<BeautySNS.Domain.Model.FriendInvitation> friendInvitations)
         {
            FriendInvitations = friendInvitations;
         }

        public IEnumerable<BeautySNS.Domain.Model.FriendInvitation> FriendInvitations { get; set; }

        public bool userSession { get; set; }
        public int accountID { get; set; }
        public Account LoggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public string fullName { get; set; }
        public int friendInvitationID { get; set; }
        public bool adminUser { get; set; }
    }
}