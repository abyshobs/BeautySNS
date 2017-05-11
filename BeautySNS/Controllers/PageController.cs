using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Admin.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySNS.Controllers
{
    public class PageController : Controller
    {
        private IUserSession userSession;
        private IAccountPermissionDAO accountPermissionDAO;

        public PageController(IUserSession userSession, IAccountPermissionDAO accountPermissionDAO)
        {
            this.userSession = userSession;
            this.accountPermissionDAO = accountPermissionDAO;
        }
        
        public ActionResult AboutUs()
        {
            PageViewModel model = new PageViewModel();

            if (userSession.LoggedIn == false)
            {
                model.userSession = false;
            }
           
            Account account = userSession.CurrentUser;                      
            if (account != null &&  userSession.LoggedIn == true)
            {
                var adminUser = accountPermissionDAO.FetchByEmail(account.email);
                if (adminUser != null)
                {
                    model.adminUser = true;
                    model.userSession = true;
                }
                if(adminUser == null)
                {
                    model.userSession = true;
                }
                model.loggedInAccount = account;
                model.loggedInAccountID = account.accountID;
            }

            else if(account == null)
            {
                model.userSession = false;
                model.adminUser = false;
            }
           
            
            return View(model);
        }

        public ActionResult FAQs()
        {
            Account account = userSession.CurrentUser;
            PageViewModel model = new PageViewModel();

            if (account != null)
            {
                var adminUser = accountPermissionDAO.FetchByEmail(account.email);
                if (adminUser != null && userSession.LoggedIn == true)
                {
                    model.adminUser = true;
                    model.userSession = true;
                }
                if (adminUser == null)
                {
                    model.userSession = true;
                }
                model.loggedInAccount = account;
                model.loggedInAccountID = account.accountID;
            }

            else if (account == null)
            {
                model.userSession = false;
                model.adminUser = false;
            }


            return View(model);
        }

        public ActionResult PrivacyPolicy()
        {
            Account account = userSession.CurrentUser;
            PageViewModel model = new PageViewModel();

            if (account != null && userSession.LoggedIn == true)
            {
                var adminUser = accountPermissionDAO.FetchByEmail(account.email);
                if (adminUser != null)
                {
                    model.adminUser = true;
                    model.userSession = true;
                }
                if (adminUser == null)
                {
                    model.userSession = true;
                }
                model.loggedInAccount = account;
                model.loggedInAccountID = account.accountID;
            }

            else if (account == null)
            {
                model.userSession = false;
                model.adminUser = false;
            }


            return View(model);
        }

        public ActionResult SiteTerms()
        {
            Account account = userSession.CurrentUser;
            PageViewModel model = new PageViewModel();

            if (account != null && userSession.LoggedIn == true)
            {
                var adminUser = accountPermissionDAO.FetchByEmail(account.email);
                if (adminUser != null)
                {
                    model.adminUser = true;
                    model.userSession = true;
                }
                if (adminUser == null)
                {
                    model.userSession = true;
                }
                model.loggedInAccount = account;
                model.loggedInAccountID = account.accountID;
            }

            else if (account == null)
            {
                model.userSession = false;
                model.adminUser = false;
            }


            return View(model);
        }

    }
}