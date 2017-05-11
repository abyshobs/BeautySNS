using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BeautySNS.Domain.Code
{
    public class SessionWrapper : ISessionWrapper
    {
        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public bool ContainsInSession(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

        public void RemoveFromSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        private string GetQueryStringValue(string key)
        {
            string a = "";
            a = a.Replace(" ", "+");
            int mod4 = a.Length % 4;
            if (mod4 > 0)
            {
                a += new string('=', 4 - mod4);
            }
            return HttpContext.Current.Request.QueryString.Get(key);
        }

        private void SetInSession(string key, object value)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return;
            }

            HttpContext.Current.Session[key] = value;
        }

        private object GetFromSession(string key)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return null;
            }

            return HttpContext.Current.Session[key];
        }

        private void UpdateInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public bool LoggedIn
        {
            get
            {
                if (ContainsInSession("LoggedIn"))
                {
                    if ((bool)GetFromSession("LoggedIn"))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                SetInSession("LoggedIn", value);
            }
        }

        public Account CurrentUser
        {
            get
            {
                if (ContainsInSession("CurrentUser"))
                {
                    return GetFromSession("CurrentUser") as Account;
                }

                return null;
            }
            set
            {
                SetInSession("CurrentUser", value);
            }
        }


        public string Email
        {
            get
            {
                if (ContainsInSession("Email"))
                {
                    return GetFromSession("Email").ToString();
                }

                return "";
            }

            set
            {
                SetInSession("Email", value);
            }
        }

        public string EmailToVerify
        {
            get
            {

                return GetQueryStringValue("a").ToString();
            }
        }

        public int AccountID
        {
            get
            {
                if (!string.IsNullOrEmpty(GetQueryStringValue("AccountID")))
                {
                    return Convert.ToInt32(GetQueryStringValue("AccountID"));
                }
                return 0;
            }
        }

        public string FriendshipRequest
        {
            get
            {
                string result;
                if (!string.IsNullOrEmpty(GetQueryStringValue("InvitationKey")))
                {
                    result = GetQueryStringValue("InvitationKey");
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        public int accountIDToInvite
        {
            get
            {
                int result;
                if (!string.IsNullOrEmpty(GetQueryStringValue("accountIDToInvite")))
                {
                    result = Convert.ToInt32(GetQueryStringValue("accountIDToInvite"));
                }
                else
                {
                    result = 0;
                }
                return result;
            }
        }

    }
}


