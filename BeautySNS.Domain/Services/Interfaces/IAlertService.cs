using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Services.Interfaces
{
    public interface IAlertService
    {

          void AddAccountCreatedAlert();
          void AddAccountModifiedAlert(Account modifiedAccount);
          void AddProfileCreatedAlert();
          void AddProfileModifiedAlert();      
          //void AddFriendRequestAlert(Account friendRequestFrom, Account friendRequestTo, Guid requestGuid, string message);
          //void AddFriendAddedAlert(Account friendRequestFrom, Account friendRequestTo);
          void AddStatusUpdateAlert(StatusUpdate statusUpdate);
          void AddPermissionCreatedAlert(Permission permission);
          void AddAdminUserCreatedAlert(AccountPermission accountPermission);
          void JobCreatedAlert(Job job);
          void JobUpdatedAlert(Job modifiedJob);
          void AdminUpdatedAlert(AccountPermission modifiedPermission);
          void AdminUserRemovedAlert(AccountPermission accountPermission);
          void AccountRemovedAlert(Account account);
          void StatusUpdateRemovedAlert(StatusUpdate statusUpdate);

    }  
    }