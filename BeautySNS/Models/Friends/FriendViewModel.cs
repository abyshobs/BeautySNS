using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Friends
{
    public class FriendViewModel
    {
          public FriendViewModel()
         {
         }

         public FriendViewModel(Friend friend)
         {
             friendID = friend.friendID;
         }


         public FriendViewModel(IEnumerable<BeautySNS.Domain.Model.Friend> friends)
         {
            Friends = friends;
         }

         public FriendViewModel(IEnumerable<BeautySNS.Domain.Model.Account> accounts)
         {
             Accounts = accounts;
         }
         public IEnumerable<BeautySNS.Domain.Model.Account> Accounts { get; set; }

        public IEnumerable<BeautySNS.Domain.Model.Friend> Friends { get; set; }

        public bool userSession { get; set; }
        public int accountID { get; set; }
        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public string fullName { get; set; }
        public int friendID { get; set; }
    }
}