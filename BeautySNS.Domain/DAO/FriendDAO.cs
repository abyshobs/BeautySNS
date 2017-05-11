using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO
{
    public class FriendDAO : IFriendDAO
    {
        //creates an instance of the database
            private readonly BSNSContext _db;

            public FriendDAO(BSNSContext db)
            {
                _db = db;
            }

            //fetches all friend relationships
            public Friend FetchFriendByID(int friendID)
            {
                Friend result = _db.Friends.Where(f => f.friendID == friendID).FirstOrDefault();
                return result;

            }

            public Friend FetchAFriend(int accountID)
            {
                Friend result = _db.Friends.Where(f => f.accountID == accountID && f.myFriendsAccountID != accountID
                                                  || f.accountID != accountID && f.myFriendsAccountID == accountID).FirstOrDefault();
                return result;
            }

            //lists out the friend relationships
            public List<Friend> FetchFriendsByAccountID(int accountID)
            {
                //shows all friendship owned by the account
                List<Friend> result = new List<Friend>();
                IEnumerable<Friend> friends = _db.Friends.Where(f => f.accountID == accountID &&
                                                                f.myFriendsAccountID != accountID).Distinct();
                result = friends.ToList();

                //also shows friendships not owned by the account
                var friends2 = _db.Friends.Where(f => f.myFriendsAccountID == accountID && 
                                                      f.accountID != accountID);
                                friends2.Select(x=> new{x.friendID, x.myFriendsAccountID, x.accountID, x.createDate}).Distinct();
    
                // adds all the friends together and returns them in a list
                foreach (var y in friends2)
                {
                  Friend friend = new Friend()
                     {
                       friendID = y.friendID,
                       accountID = y.accountID,
                       createDate = y.createDate,
                       myFriendsAccountID = y.myFriendsAccountID
                     };
                    
                       result.Add(friend);
               }
                return result;
            }

            public List<Account> FetchFriendsAccountByAccountID(int accountID)
            {
                //list out all friends
                List<Friend> friends = FetchFriendsByAccountID(accountID);
                List<int> accountIDs = new List<int>();
                List<int> friendsIDs = new List<int>();
                
                //assign the myFriendsAccountID to the list of accountIDs 
                foreach (Friend friend in friends)
                {
                    if(accountID == friend.myFriendsAccountID)
                    {
                        accountIDs.Add(friend.accountID);
                    }

                    else if(accountID != friend.myFriendsAccountID)
                    {
                        friendsIDs.Add(friend.myFriendsAccountID);
                    }
                    //accountIDs.Add(friend.myFriendsAccountID);
                }

                //from the list of accountIDs, return accounts that also contains the current accountID. This shows the two way relationship between the friends
                List<Account> result = new List<Account>();
                IEnumerable<Account> accounts = from a in _db.Accounts
                                                where accountIDs.Contains(a.accountID) || friendsIDs.Contains(a.accountID)
                                                select a;                 
                result = accounts.ToList();
                return result; //return the bi-directional friendship accounts
            }

        //creates the friendship
            public void SaveFriend(Friend friend)
            {
                    friend.createDate = DateTime.Now;
                    _db.Friends.Add(friend);                   
                    _db.SaveChanges();
                   
            }

        //deletes friendship
            public void RemoveFriend(int id)
            {
                var _friend = FetchFriendByID(id);
                Friend friend = _db.Friends.Find(_friend.friendID);
                _db.Friends.Remove(friend);
                _db.SaveChanges();            
            }

            public void RemoveFriendsByID(int removerAccountID, int friendIDToRemove)
            {
                //fetches and returns a list of friends to remove
                List<Friend> workingList = new List<Friend>();
                IEnumerable<Friend> friends = from f in _db.Friends
                                              where (f.accountID == removerAccountID &&
                                              f.myFriendsAccountID == friendIDToRemove) ||
                                              (f.accountID == friendIDToRemove &&
                                              f.myFriendsAccountID == removerAccountID)
                                              select f;
                workingList = friends.ToList();

                foreach (Friend friend in workingList)
                {
                    RemoveFriend(friend.friendID);
                    _db.SaveChanges();
                }
            }

        //removes a single friendship
            public void RemoveFriendByID(int removerAccountID, int friendIDToRemove)
            {
             
               Friend friendship = _db.Friends.Where(f => f.accountID == removerAccountID &&
                             f.myFriendsAccountID == friendIDToRemove || f.accountID == friendIDToRemove &&
                             f.myFriendsAccountID == removerAccountID).FirstOrDefault();
                                   
                RemoveFriend(friendship.friendID);
            }

        public bool IsFriend(Account accountBeingViewed, Account account)
        {
            if (account == null)
                    return false;

                if (accountBeingViewed == null)
                    return false;

               Friend friend = FetchFriendsByAccountID(account.accountID).Where(f => f.accountID == account.accountID && f.myFriendsAccountID == accountBeingViewed.accountID).FirstOrDefault();
               if (friend != null)                 
                return true;
               else               
                return false;
        }

    }
}