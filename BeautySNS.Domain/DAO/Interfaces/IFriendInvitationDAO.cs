using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IFriendInvitationDAO
    {
       List<FriendInvitation> FetchByAccountID(int accountID);
       FriendInvitation FetchByGUID(Guid guid);
       void CreateInvitation(FriendInvitation friendInvitation);
       FriendInvitation InviteFirend(int accoundIDToInvite);
       FriendInvitation FetchById(int accoundID);
       void UpdateInvitation(FriendInvitation invitation, Account invitationTo);
       List<FriendInvitation> FetchPendingRequests(Account account);
       FriendInvitation FetchByID(int id);
       void DeleteInvitation(int id);
       FriendInvitation FetchSentInvitation(Account account1, Account account2);
    }
}
