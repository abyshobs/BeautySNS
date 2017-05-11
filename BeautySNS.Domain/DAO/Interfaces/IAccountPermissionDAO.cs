using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IAccountPermissionDAO
    {
        void CreatePermission(Permission permission);
        IEnumerable <Permission> FetchAllPermissions ();
        IEnumerable<AccountPermission> FetchAllAccountPermissions();
        Permission FetchPermissionByID(int id);
        Permission FetchPermissionByName(string name);
        void CreateAccountPermission(AccountPermission accountPermission);
        AccountPermission FetchAccountPermissionByID(int id);
        void updateAccountPermission(AccountPermission accountPermission);
        void DeleteAccountPermission(int id);
        AccountPermission FetchByEmail(string email);
    
    }
}
