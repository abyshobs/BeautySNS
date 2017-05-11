using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Services
{
    public class AlertService : IAlertService
    {
       
        //creates an instance of the database and the interfaces whose methods will be used
        private readonly BSNSContext _db;
        private IFriendDAO friendDAO;
        private IUserSession userSession;
        private IAlertDAO alertDAO;

        Alert alert = new Alert();

        public AlertService(BSNSContext db, IFriendDAO friendDAO, IUserSession userSession, IAlertDAO alertDAO)
        {
            _db = db;
            this.friendDAO = friendDAO;
            this.userSession = userSession;
            this.alertDAO = alertDAO;
        }

        //use the DAO to save the alert
        private void SaveAlert(Alert alert)
        {
            alertDAO.CreateAlert(alert);
        }

       
      //creates an alert when a user creates an account
        public void AddAccountCreatedAlert()
        {
            Account account = userSession.CurrentUser; 
            var alertMessage = "<div>" + account.email + "just signed up!</div>";

            if(userSession.CurrentUser != null)
                alert.accountID = account.accountID;

            alert.createDate = DateTime.Now;
            alert.alertTypeID = (int)AlertType.AlertTypes.AccountCreated;
            SaveAlert(alert);

        }

        //creates an alert when a user modifies their account details
        public void AddAccountModifiedAlert(Account modifiedAccount)
        { 
           var account = modifiedAccount;
           var alertMessage = account.email + " modified their account.";
           
           alert.accountID = modifiedAccount.accountID;
           alert.createDate = DateTime.Now;
           
           alert.alertTypeID = (int)AlertType.AlertTypes.AccountModified;
           alert.message = alertMessage;

           SaveAlert(alert);
           SendAlertToFriends(alert);
           

        }

        //creates an alert when a user creates their profile
        public void AddProfileCreatedAlert()
        {
            Account account = userSession.CurrentUser;
            var alertMessage = "" + account.email + " just created their profile! ";

            if(userSession.CurrentUser != null)
                alert.accountID = account.accountID;

            alert.createDate = DateTime.Now;
            alert.message = alertMessage;
            alert.alertTypeID = (int) AlertType.AlertTypes.ProfileCreated;

            SaveAlert(alert);
            SendAlertToFriends(alert);
            
        }

        //creates an alert when a user modifies their profile
        public void AddProfileModifiedAlert()
        {
            Account account = userSession.CurrentUser;
            var alertMessage = "" + account.email + " just created changed their profile details!";

            if(userSession.CurrentUser != null)
                alert.accountID = account.accountID;

            alert.createDate = DateTime.Now;
            alert.message = alertMessage;
            alert.alertTypeID = (int) AlertType.AlertTypes.ProfileModified;
        
            SaveAlert(alert);
            SendAlertToFriends(alert);
        }

        //send friend request   
       //  public void AddFriendRequestAlert(Account friendRequestFrom, Account friendRequestTo, Guid requestGuid, string message)
       //  {
       //      var alertMessage = "<div>" + friendRequestFrom.accountID + " would like to be friends!</div>";
       //      alertMessage += friendRequestFrom.firstName + " " + friendRequestFrom.lastName +
       //                     " would like to be friends with you!  Click this link to accept the request: ";
       //      alert.createDate = DateTime.Now;
       //      alert.accountID = friendRequestTo.accountID;
       //      alert.message = alertMessage;
       //      alert.alertTypeID = (int) AlertType.AlertTypes.FriendRequest;
       //      SaveAlert(alert);
       //  }

       ////became friends

       //  public void AddFriendAddedAlert(Account friendRequestFrom, Account friendRequestTo)
       //  {
       //     alert = new Alert();
       //     alert.accountID = friendRequestFrom.accountID;
       //     alert.createDate = DateTime.Now;
       //     var alertMessage = "<div>" + friendRequestTo.accountID +
       //                                  friendRequestTo.email + " is now your friend!</div>";
       //     alertMessage += "<div>" + friendRequestTo.accountID + "</div>";
       //     alert.message = alertMessage;
       //     alert.alertTypeID = (int)AlertType.AlertTypes.FriendAdded;
       //     SaveAlert(alert);

       //     alert = new Alert();
       //     alert.createDate = DateTime.Now;
       //     alert.accountID = friendRequestTo.accountID;
       //     alertMessage = "<div>" + friendRequestFrom.accountID +
       //                    friendRequestFrom.email + " is now your friend!</div>";
       //     alertMessage += "<div class=\"AlertRow\">" + friendRequestFrom.accountID + "</div>";
       //     alert.message = alertMessage;
       //     alert.alertTypeID = (int)AlertType.AlertTypes.FriendAdded;
       //     SaveAlert(alert);

            
       //     alert = new Alert();
       //     alert.createDate = DateTime.Now;
       //     alert.alertTypeID = (int) AlertType.AlertTypes.FriendAdded;
       //     alertMessage = "<div>" + friendRequestFrom.email + " and " +
       //                    friendRequestTo.email + " are now friends!</div>";
       //     alert.message = alertMessage;
       //     SaveAlert(alert);

       //     alert.accountID = friendRequestFrom.accountID;
       //     SendAlertToFriends(alert);

       //     alert.accountID = friendRequestTo.accountID;
       //     SendAlertToFriends(alert);

       //  }

         public void AddStatusUpdateAlert(StatusUpdate statusUpdate)
         { 
             Alert alert = new Alert();
             alert.createDate = DateTime.Now;
             alert.accountID = userSession.CurrentUser.accountID;
             alert.alertTypeID = (int)AlertType.AlertTypes.StatusUpdate;
             
             var alertMessage =  userSession.CurrentUser.email + "wrote: " + statusUpdate.status; 
             alert.message = alertMessage;
         
             SendAlertToFriends(alert);
             SaveAlert(alert);
         }
          
        //changes profile pic
        //public void AddNewAvatarAlert()
        //{
        //   Account account = userSession.CurrentUser;
        //    var alertMessage = "<div>" + account.email + "just created changed their profile details!</div>";

        //    if(userSession.CurrentUser != null)
        //        alert.accountID = account.accountID;
        //    alertMessage =
        //        "<div class=\"AlertHeader\">" + GetProfileImage(account.AccountID) + GetProfileUrl(account.Username) +
        //        " added a new avatar.</div>";

        //    alert.message = alertMessage;
        //    alert.alertTypeID = (int) AlertType.AlertTypes.NewAvatar;
        //    CreateAlert(alert);
        //    SendAlertToFriends(alert);

        //}     
         
        //add admin user
        //write status update

         private void SendAlertToFriends(Alert alert)
         {
             List<Friend> friends = friendDAO.FetchFriendsByAccountID(alert.accountID);

             foreach (Friend friend in friends)
             {
                 if (alert.accountID == friend.accountID && alert.accountID != friend.myFriendsAccountID)
                 {                   
                     alert.accountID = friend.myFriendsAccountID;
                     SaveAlert(alert);                                   
                 }

                 if (alert.accountID == friend.myFriendsAccountID && alert.accountID != friend.accountID)
                 {
                     alert.accountID = friend.accountID;
                     SaveAlert(alert);         
                 }
             }
         }


         public void AddPermissionCreatedAlert(Permission permission)
         {
             Alert alert = new Alert();
             alert.createDate = DateTime.Now;
             alert.accountID = userSession.CurrentUser.accountID;
             alert.alertTypeID = (int)AlertType.AlertTypes.PermissionCreated;

             var alertMessage = userSession.CurrentUser.email + "added a new permission to the system : " + permission.name;
             alert.message = alertMessage;

             SaveAlert(alert);
         }


         public void AddAdminUserCreatedAlert(AccountPermission accountPermission)
         {
             Alert alert = new Alert();
             alert.createDate = DateTime.Now;
             alert.accountID = userSession.CurrentUser.accountID;
             alert.alertTypeID = (int)AlertType.AlertTypes.AdminCreated;

             var alertMessage = userSession.CurrentUser.email + "added a new admin user to the system : " + accountPermission.email;
             alert.message = alertMessage;

             SaveAlert(alert);
         }

         public void JobCreatedAlert(Job job)
         {
             Alert alert = new Alert();
             alert.createDate = DateTime.Now;
             alert.accountID = userSession.CurrentUser.accountID;
             alert.alertTypeID = (int)AlertType.AlertTypes.JobCreated;

             var alertMessage = userSession.CurrentUser.email + "added a new job to the system : " + job.name ;
             alert.message = alertMessage;

             SaveAlert(alert);
         }

         public void AdminUpdatedAlert(AccountPermission modifiedPermission)
         {   
           Account account = userSession.CurrentUser;
           var accountPermission = modifiedPermission;
           var alertMessage = account.email + " modified " + modifiedPermission.email + "'s permission";
          
           alert.accountID = account.accountID;
           alert.createDate = DateTime.Now;
           
           alert.alertTypeID = (int)AlertType.AlertTypes.AdminUpdated;
           alert.message = alertMessage;

           SaveAlert(alert);
        }
         
         public void JobUpdatedAlert(Job modifiedJob)
         {
             Account account = userSession.CurrentUser;
             var job = modifiedJob;
             var alertMessage = account.email + " modified a job : " + modifiedJob.name;

             alert.accountID = account.accountID;
             alert.createDate = DateTime.Now;

             alert.alertTypeID = (int)AlertType.AlertTypes.JobUpdated;
             alert.message = alertMessage;

             SaveAlert(alert);
         }

         public void AdminUserRemovedAlert(AccountPermission accountPermission)
         {
             Account account = userSession.CurrentUser;
             var alertMessage = account.email + " removed admin user : " + accountPermission.email;

             alert.accountID = account.accountID;
             alert.createDate = DateTime.Now;

             alert.alertTypeID = (int)AlertType.AlertTypes.AdminRemoved;
             alert.message = alertMessage;

             SaveAlert(alert);
         }

         public void AccountRemovedAlert(Account account)
         {
             Account _account = userSession.CurrentUser;
             var alertMessage = _account.email + " removed : " + account.firstName + account.lastName + "from the system";

             alert.accountID = _account.accountID;
             alert.createDate = DateTime.Now;

             alert.alertTypeID = (int)AlertType.AlertTypes.AccountRemoved;
             alert.message = alertMessage;

             SaveAlert(alert);
         }

        public void StatusUpdateRemovedAlert(StatusUpdate statusUpdate)
         {
             Account _account = userSession.CurrentUser;
             var alertMessage = _account.email + " removed : " + statusUpdate.status;

             alert.accountID = _account.accountID;
             alert.createDate = DateTime.Now;

             alert.alertTypeID = (int)AlertType.AlertTypes.StatusUpdateRemoved;
             alert.message = alertMessage;

             SaveAlert(alert);

         }

    }
}

