using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IAccountDAO
    {
        void CreateAccount(Account account);
        void Update(Account account);
        Account FetchById(int id);
        Account FetchByEmail(string email);
        void Delete(int id);
        List<Account> SearchAccounts(string SearchText);
        IEnumerable<Account> FetchAllUserAccounts();
    }
}
