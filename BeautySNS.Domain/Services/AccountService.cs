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
    public class AccountService : IAccountService
    {
        private IAccountDAO accountDAO;
        private ISessionWrapper sessionWrapper;
        private IEmail email;
        private IUserSession userSession;

        public AccountService(IAccountDAO accountDAO, ISessionWrapper sessionWrapper, IEmail email, IUserSession userSession)
        {
            this.accountDAO = accountDAO;
            this.sessionWrapper = sessionWrapper;
            this.email = email;
            this.userSession = userSession;
        }

        public bool EmailInUse(string email)
        {
            Account account = accountDAO.FetchByEmail(email);
            {
                if (account != null) { return true; }
                return false;
            }
        }      

        public void Logout() 
        { 
            userSession.LoggedIn = false;
            userSession.CurrentUser = null; 
        }
       
    }
}
