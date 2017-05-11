using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO
{
    public class FriendInvitationDAO : IFriendInvitationDAO
    {
        //creates an instance of the database
        private readonly BSNSContext _db;

        public FriendInvitationDAO(BSNSContext db)
        {
            _db = db;
        }


        //lists out all the invitations sent out by the account
        public List<FriendInvitation> FetchByAccountID(int accountID)
        {
            List<FriendInvitation> result = new List<FriendInvitation>();
            IEnumerable<FriendInvitation> friendInvitations = _db.FriendInvitations.Where(fi => fi.accountID == accountID);
            result = friendInvitations.ToList();
            return result;
        }

        public FriendInvitation FetchById(int accountID)
        {
            return _db.FriendInvitations.FirstOrDefault(f => f.accountID == accountID);
        }

        public FriendInvitation FetchByID(int id)
        {
            return _db.FriendInvitations.FirstOrDefault(f => f.friendInvitationID == id);
        }

        public FriendInvitation FetchSentInvitation(Account account1,  Account account2)
        {
            return _db.FriendInvitations.Where(fi => fi.accountID == account1.accountID && fi.email == account2.email
                                                     || fi.accountID == account2.accountID && fi.email == account1.email
                                                     ).FirstOrDefault();
        }


        //create a method to fetch pending invitations !!
        public List<FriendInvitation> FetchPendingRequests(Account account)
        {
            List<FriendInvitation> result = new List<FriendInvitation>();
            IEnumerable<FriendInvitation> friendInvitations = _db.FriendInvitations.Where(f => f.email == account.email 
                                                               && f.becameAccountID == 0).Distinct();
            result = friendInvitations.ToList();
            return result;
        }

        //fetches the invitation by its GUid (unique identifier)
        public FriendInvitation FetchByGUID(Guid guid)
        {
            FriendInvitation friendInvitation = _db.FriendInvitations.Where(fi => fi.GUID == guid).FirstOrDefault();
            return friendInvitation;

        }

        //create a friend invitation
        public void CreateInvitation(FriendInvitation friendInvitation)
        { 
              _db.FriendInvitations.Add(friendInvitation);
              friendInvitation.createDate = DateTime.Now;
                _db.SaveChanges();       
        }

        //update an invitation
        public void UpdateInvitation(FriendInvitation invitation, Account invitationTo)
        {
            FriendInvitation originalInvite = _db.FriendInvitations.Find(invitation.friendInvitationID);
            originalInvite.becameAccountID = invitationTo.accountID;

            _db.SaveChanges();
        }

        public FriendInvitation InviteFirend(int accoundIDToInvite)
        {
            return _db.FriendInvitations.FirstOrDefault(a => a.accountID == accoundIDToInvite);
            //SendInvitation(accoundIDToInvite);
            //Redirect("~/Friend/SendInvitation?accountIDToInvite=" + accoundIDToInvite.ToString());
        }

        public void DeleteInvitation(int id)
        {
            FriendInvitation invitation = _db.FriendInvitations.Find(id);
            _db.FriendInvitations.Remove(invitation);
            _db.SaveChanges();
        }


    }
}
