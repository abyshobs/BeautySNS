using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IFriendDAO
    {
        Friend FetchFriendByID(int friendID);
        List<Friend> FetchFriendsByAccountID(int accountID);
        List<Account> FetchFriendsAccountByAccountID(int accountID);     
        void SaveFriend(Friend friend);       
        void RemoveFriend(int id);        
        void RemoveFriendByID(int removerAccountID, int friendIDToRemove);
        bool IsFriend(Account account, Account accountBeingViewed);
        Friend FetchAFriend(int accountID);
        
    }
}
