using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Code
{
    public class UserSession : IUserSession
    {
        private ISessionWrapper _sessionWrapper;

        public UserSession(ISessionWrapper sessionWrapper)
        {
            this._sessionWrapper = sessionWrapper;
        }

        public bool LoggedIn
        {
            get
            {
                return _sessionWrapper.LoggedIn;
            }
            set
            {
                _sessionWrapper.LoggedIn = value;
            }
        }

        public Account CurrentUser
        {
            get
            {
                return _sessionWrapper.CurrentUser;
            }
            set
            {
                _sessionWrapper.CurrentUser = value;
            }
        }

        public string Email
        {
            get
            {
                return _sessionWrapper.Email;
            }

            set
            {
                _sessionWrapper.Email = value;
            }
        }

    }
}

 