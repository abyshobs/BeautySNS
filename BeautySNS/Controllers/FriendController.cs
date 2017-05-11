using BeautySNS.Admin.Models.Friends;
using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySNS.Controllers
{
    public class FriendController : Controller
    {
        private IAccountDAO accountDAO;
        private IProfileDAO profileDAO;
        private IFriendDAO friendDAO;
        private IFriendInvitationDAO friendInvitationDAO;
        private IEmail emails;
        private IUserSession userSession;
        private ISessionWrapper sessionWrapper;
        private Account _account;
        private Account _accountToInvite;


        public FriendController(IAccountDAO accountDAO, IProfileDAO profileDAO,IFriendDAO friendDAO, IFriendInvitationDAO friendInvitationDAO, IEmail emails, IUserSession userSession, ISessionWrapper sessionWrapper, Account _account, Account _accountToInvite)
     {
       this.accountDAO = accountDAO;
       this.profileDAO = profileDAO;
       this.friendDAO = friendDAO;
       this.friendInvitationDAO = friendInvitationDAO;
       this.emails = emails;
       this.userSession = userSession;
       this.sessionWrapper = sessionWrapper;
       this._account = _account;
       this._accountToInvite = _accountToInvite;
     }

        [HttpGet]
        public ActionResult SendRequest(int id = 0)
        {
            Account account = userSession.CurrentUser;
            var accountToInvite = accountDAO.FetchById(id);

            var invitation = friendInvitationDAO.FetchSentInvitation(account, accountToInvite);

            if(invitation == null)
            {
                SendInvitation(accountToInvite);
                TempData["successMessage"] = "Network Request has been sent";
            }

            else if (invitation != null)
            {
                TempData["errorMessage"] = "There is already a pending invitation between you and this user";
            }
            
            return RedirectToAction("UserProfileHomepage", "Profile", new { id = accountToInvite.accountID});
        }
        
        //send a friend request
        public ActionResult SendInvitation(Account accountToInvite)
        {
            Account account = userSession.CurrentUser;
            if(account!= null)
            {              
                if (accountToInvite.accountID > 0)
                {
                    accountToInvite = accountDAO.FetchById(accountToInvite.accountID);
                    if (accountToInvite != null)
                    {
                        SendInvitationMessage(accountToInvite.email,
                                          account.firstName + " " + account.lastName + " would like to be your friend!");
                    }
                }
              }
            
           return View();
        }
     
       //sends invitation message 
        public void SendInvitationMessage(string ToEmailArray, string Message)
        {
            string resultMessage = "Invitations sent to the following recipients:<BR>";
            resultMessage += emails.SendInvitations(userSession.CurrentUser,ToEmailArray, Message);
        }

        public ActionResult ConfirmNetworkRequest(int id = 0)
        {
            var invitedAccount = userSession.CurrentUser;
            FriendInvitation invitation = friendInvitationDAO.FetchByID(id);
                if (invitation != null)
                {
                    if (sessionWrapper.CurrentUser != null)
                    {
                        accountDAO.FetchById(invitedAccount.accountID);
                        CreateFriendFromFriendInvitation(invitation.GUID, invitedAccount);                       
                        TempData["successMessage"] = "You are now friends";
                    }
                    else
                    {
                        TempData["errorMessage"] = "There was an error";
                    }
                }
                return RedirectToAction("PendingRequests");
        }

        public ActionResult RejectNetworkRequest(int id = 0)
        {
            var invitedAccount = userSession.CurrentUser;
            FriendInvitation invitation = friendInvitationDAO.FetchByID(id);
                if (invitation != null)
                {
                    if (sessionWrapper.CurrentUser != null)
                    {
                        friendInvitationDAO.DeleteInvitation(invitation.friendInvitationID);                     
                    }
                    else
                    {
                        TempData["errorMessage"] = "There was an error";
                    }
                }
                return RedirectToAction("PendingRequests");
        }

        
        [HttpPost]
        public void ConfirmFriendRequest()
        {
            var invitedAccount = userSession.CurrentUser;
            if (!string.IsNullOrEmpty(sessionWrapper.FriendshipRequest))
            {
                FriendInvitation friendInvitation = friendInvitationDAO.FetchByGUID(new Guid(sessionWrapper.FriendshipRequest));
                {
                    if (friendInvitation != null)
                    {
                        if (sessionWrapper.CurrentUser != null)
                        {
                            //RedirectToAction("FriendConfirmationPage", "Friend");
                            accountDAO.FetchById(invitedAccount.accountID);
                            CreateFriendFromFriendInvitation(friendInvitation.GUID, invitedAccount);
                            TempData["successMessage"] = "You are now friends";
                        }
                        else
                        {
                            TempData["errorMessage"] = "There was an error";
                        }
                    }
                }
            }
        }


        public void CreateFriendFromFriendInvitation(Guid invitationKey, Account invitationTo)
        {
            FriendInvitation friendInvitation = friendInvitationDAO.FetchByGUID(invitationKey);
            //friendInvitation.becameAccountID = invitationTo.accountID;
            friendInvitationDAO.UpdateInvitation(friendInvitation, invitationTo);

            //create friendship
            Friend friend = new Friend();
            friend.accountID = friendInvitation.accountID;
            friend.myFriendsAccountID = invitationTo.accountID;
            friendDAO.SaveFriend(friend);

            Account invitationFrom = accountDAO.FetchById(friendInvitation.accountID);
        }

        public void RemoveUser(int id = 0)
        {
            Profile profile = profileDAO.fetchByAccountID(userSession.CurrentUser.accountID);
            Profile profileBeingViewed = profileDAO.fetchByAccountID(id);
          
            var account1 = profile.Account.accountID;
            var account2 = profileBeingViewed.Account.accountID;

            friendDAO.RemoveFriendByID(account1, account2);
        }

        public ActionResult PendingRequests(Account account)
        {
            account = userSession.CurrentUser;
            var pendingRequests = friendInvitationDAO.FetchPendingRequests(account);
            PendingRequestsViewModel model = new PendingRequestsViewModel(pendingRequests);                   
            model.userSession = userSession.LoggedIn;
            model.fullName = string.Format("{0} {1}", account.firstName, account.lastName);
            model.loggedInAccountID = account.accountID;
            model.LoggedInAccount = account;
            model.adminUser = false;
            return View(model);
        }

        public ActionResult Friends(int accountID = 0)
        {
            Account account = userSession.CurrentUser;
            accountID = account.accountID;
            var friends = friendDAO.FetchFriendsAccountByAccountID(accountID);

            BeautySNS.Admin.Models.Accounts.IndexViewModel model = new BeautySNS.Admin.Models.Accounts.IndexViewModel(friends);

            model.userSession = userSession.LoggedIn;
            model.fullName = string.Format("{0} {1}", account.firstName, account.lastName);
            model.loggedInAccountID = account.accountID;
            model.loggedInAccount = account;
            //model.adminUser = false;
            return View(model);
        }

        public ActionResult AllUserFriends(int id = 0)
        {
            Account _account = userSession.CurrentUser;

            Account account = accountDAO.FetchById(id);
            var friends = friendDAO.FetchFriendsAccountByAccountID(id);
            
            BeautySNS.Admin.Models.Accounts.IndexViewModel model = new BeautySNS.Admin.Models.Accounts.IndexViewModel(friends);

            model.userSession = userSession.LoggedIn;
            model.firstName = account.firstName;
            model.userAccountID = account.accountID;

            model.fullName = string.Format("{0} {1}", _account.firstName, _account.lastName);
            model.loggedInAccountID = _account.accountID;
            model.loggedInAccount = _account;
            return View(model);
        }
      
        public ActionResult RemoveFriend(int id = 0)
        {
            var friend = friendDAO.FetchFriendsAccountByAccountID(id);
            friendDAO.RemoveFriend(id);
            TempData["successMessage"] = "This user has been removed from your network";
            return RedirectToAction("ProfileHomepage", "Profile");
            
        }

        public ActionResult UserFriends(int id = 0)
        {
            Account account = accountDAO.FetchById(id);
            var friends = friendDAO.FetchFriendsAccountByAccountID(id);
            return PartialView(friends);
        }

        public FileContentResult getFriendImg(int id = 0)
        {
            Account account = accountDAO.FetchById(id);
            byte[] byteArray = account.Profile.avatar;
            return byteArray != null
            ? new FileContentResult(byteArray, "image/jpeg")
            : null;
        }
        
        public FileContentResult getImg(int id = 0)
        {
            Account account = userSession.CurrentUser;
            Friend friend = friendDAO.FetchFriendByID(id);

     
            if(friend.accountID == account.accountID && friend.myFriendsAccountID != account.accountID)
            {
                Account friendAccount = accountDAO.FetchById(friend.myFriendsAccountID);
                    byte[] byteArray = friendAccount.Profile.avatar;
                    return byteArray != null
                    ? new FileContentResult(byteArray, "image/jpeg")
                    : null;
            }

            if(friend.accountID != account.accountID && friend.myFriendsAccountID == account.accountID)
            {
                Account friendAccount = accountDAO.FetchById(friend.accountID);
                byte[] byteArray = friendAccount.Profile.avatar;
                return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
            }
            return null;                                               
        }


    }
    }