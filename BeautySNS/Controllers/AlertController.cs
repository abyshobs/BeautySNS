using BeautySNS.Admin.Models.Alerts;
using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySNS.Controllers
{
    public class AlertController : Controller
    {
        //initializes the repositories that will be used in this controller
        private IAlertDAO alertDAO;
        private IUserSession userSession;
        private IAccountDAO accountDAO;
        private IAccountPermissionDAO accountPermissionDAO;

        public AlertController(IAlertDAO alertDAO, IUserSession userSession, IAccountDAO accountDAO, IAccountPermissionDAO accountPermissionDAO)
        {
            this.alertDAO = alertDAO;
            this.userSession = userSession;
            this.accountDAO = accountDAO;
            this.accountPermissionDAO = accountPermissionDAO;
        }

        //shows all the non-admin alerts on the site
        public ActionResult SiteActivity()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }
           
            //prevents non admin users from accessing this page
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("This page is restricted to admin users");
            }

            //calls method in repository which fetches out all the alerts in the system
            var alerts = alertDAO.FetchAllAlerts();
            IndexViewModel model = new IndexViewModel(alerts);

            model.adminUser = true;
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;

            return View(model);
        }

        public ActionResult AdminActivity()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }
            Account account = userSession.CurrentUser;
            var _adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (_adminUser == null)
            {
                return Content("This page is restricted to super admin users");
            }

            if (_adminUser != null && _adminUser.Permission.name != "SuperAdmin")
            {
                TempData["errorMessage"] = "This page is only available to super admin users";
                return RedirectToAction("SiteActivity", "Alert");
                //return Content("This page is restricted to super admin users");
            }

            else
            {
                var adminUsers = accountPermissionDAO.FetchAllAccountPermissions();
                foreach (var adminUser in adminUsers)
                {
                    var adminAlerts = alertDAO.FetchAlertsByAccountID(adminUser.accountID);
                    IndexViewModel model = new IndexViewModel(adminAlerts);

                    model.adminUser = true;
                    model.userSession = userSession.LoggedIn;
                    model.loggedInAccount = account;
                    model.loggedInAccountID = account.accountID;
                    model.permissionType = _adminUser.Permission.name;

                    return View(model);
                }
            }
            return View();
        }

        public ActionResult NewsFeed()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }
          
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser != null)
            {
                return Content("This page is restricted to super admin users");
            }
            var _account = alertDAO.FetchAlertsByAccountID(account.accountID);

            IndexViewModel model = new IndexViewModel(_account);
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.adminUser = false;
            return View(model);
        }

      public ActionResult Delete (int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);

            //Alert alert = alertDAO.FetchById(id);
            alertDAO.DeleteAlert(id);

            if (adminUser == null)
            {
                TempData["successMessage"] = "Update has been deleted";
                return RedirectToAction("NewsFeed", "Alert");
            }
            
            else if (adminUser != null)
            {
                TempData["successMessage"] = "Update has been deleted";
                return RedirectToAction("SiteActivity", "Alert");
            }

            return View();
            
        }
    }
}