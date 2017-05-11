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
    public class AccountPermissionDAO : IAccountPermissionDAO
    {
         //creates an instance of the database
        private readonly BSNSContext _db;
        private IAlertService alertService;

        public AccountPermissionDAO(BSNSContext db, IAlertService alertService)
        {
            _db = db;
        }

        //create a permission
        public void CreatePermission(Permission permission)
        {
           _db.Permissions.Add(permission);
           _db.SaveChanges();
           //alertService.AddPermissionCreatedAlert(permission);
        }

        //fetch all permissions
        public IEnumerable <Permission> FetchAllPermissions ()
        {
            return _db.Permissions.ToList();
        }

        //fetch all account permissions
        public IEnumerable<AccountPermission> FetchAllAccountPermissions()
        {
            return _db.AccountPermissions.ToList();
        }

        //fetch permission by its id
        public Permission FetchPermissionByID(int id)
        {
          return _db.Permissions.FirstOrDefault(p => p.permissionID == id);
        }

        //fetch permission by its name
        public Permission FetchPermissionByName (string name)
        {
            return _db.Permissions.FirstOrDefault(p => p.name == name);
        }

        public AccountPermission FetchByEmail(string email)
        {
            return _db.AccountPermissions.FirstOrDefault(ap => ap.email == email);
        }

        //create an account permission
        public void CreateAccountPermission(AccountPermission accountPermission)
        {
            //accountPermission.accountID = 32;
            accountPermission.createDate = DateTime.Now;
            _db.AccountPermissions.Add(accountPermission);            
            _db.SaveChanges();
        }

        //fetch account permission by id
        public AccountPermission FetchAccountPermissionByID(int id)
        {
            return _db.AccountPermissions.FirstOrDefault(p => p.accountPermissionID == id);
        }

        //update an existing account permission
        public void updateAccountPermission(AccountPermission accountPermission)
        {
            AccountPermission originalAccountPermission = _db.AccountPermissions.Find(accountPermission.accountPermissionID);
            originalAccountPermission.accountPermissionID = accountPermission.accountPermissionID;
            originalAccountPermission.accountID = accountPermission.accountID;
            originalAccountPermission.permissionID = accountPermission.permissionID;
            originalAccountPermission.email = originalAccountPermission.email;
            //originalAccountPermission.Permission.name = accountPermission.Permission.name;
            originalAccountPermission.lastUpdateDate = DateTime.Now;                   
            _db.SaveChanges();
        }
         
        //delete an account permission
        public void DeleteAccountPermission(int id)
        {
          AccountPermission accountPermission = _db.AccountPermissions.Find(id);
          _db.AccountPermissions.Remove(accountPermission);
          _db.SaveChanges();

        }

    }
}
