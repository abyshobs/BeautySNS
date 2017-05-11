   using BeautySNS.Admin.Models.Admins;
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
    public class AdminController : Controller
    {
        private IAccountDAO accountDAO;
        private IUserSession userSession;
        private IAccountPermissionDAO accountPermissionDAO;
        private IProfileDAO profileDAO;
        private IFriendDAO friendDAO;
        private IAlertService alertService;


        public AdminController(IAccountDAO accountDAO, IUserSession userSession, IAccountPermissionDAO accountPermissionDAO, IProfileDAO profileDAO, IFriendDAO friendDAO, IAlertService alertService)
        {
            this.accountDAO = accountDAO;
            this.userSession = userSession;
            this.accountPermissionDAO = accountPermissionDAO;
            this.profileDAO = profileDAO;
            this.friendDAO = friendDAO;
            this.alertService = alertService;
        }

        public ActionResult AdminSearch(string searchString)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents user from using this search engine if they are not admin 
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);

            if (adminUser == null)
            {
                return Content("This search engine is only available to admin users");
            }
          
            else if (adminUser != null)
            {
                List<Account> accounts = accountDAO.SearchAccounts(searchString);
                if (accounts.Count == 0)
                {
                    TempData["errorMessage"] = "No search results !";
                    return RedirectToAction("SiteActivity", "Alert");
                    //return RedirectToAction("Index");
                }

                else if (accounts.Count > 0)
                {
                    //wraps the list of accounts into the index model
                    BeautySNS.Admin.Models.Accounts.IndexViewModel model = new BeautySNS.Admin.Models.Accounts.IndexViewModel(accounts);
                    
                    if(userSession.LoggedIn == true)
                    {
                        model.userSession = true;
                    }

                    else if (userSession.LoggedIn == false)
                    {
                        model.userSession = false;
                    }

                    //model.permissionType = adminUser.Permission.name;
                    model.adminUser = true;
                    model.loggedInAccount = account;
                    model.loggedInAccountID = account.accountID;
                    model.fullName = string.Format("{0} {1}", model.firstName, model.lastName);
                    return View(model);
                }
            }

            return View();
        }


        public ActionResult MyActivity() 
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents access from non admin users
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);

            if(adminUser == null)
            {
                return Content("This page is restricted to admin users.");
            }

             //fetches the account of the admin user and wraps it into the model
            else if (adminUser != null)
            {
                var _account = accountDAO.FetchById(account.accountID);
                ActivityViewModel model = new ActivityViewModel(_account);

                if (userSession.LoggedIn == true)
                {
                    model.userSession = true;
                }

                else if (userSession.LoggedIn == false)
                {
                    model.userSession = false;
                }

                model.adminUser = true;
                model.loggedInAccount = account;
                model.loggedInAccountID = account.accountID;
                model.permissionType = adminUser.Permission.name;
                return View(model);
            }
            return View();
        }

        //public view of the activity page of an admin user. 
        public ActionResult AdminUserActivity(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }
           
            //allows only super admin users to view this page
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("This page is restricted to super admin users.");
            }

            if(adminUser.Permission.name != "SuperAdmin")
            {
                return Content("This page is restricted to super admin users.");
            }

            //fetches the account being viewed and wraps it into its model
            Account userAccount = accountDAO.FetchById(id);
            if (userAccount == null)
            {
                return Content("This user does not exist");
            }

            ActivityViewModel model = new ActivityViewModel(userAccount);
            var _admin = accountPermissionDAO.FetchByEmail(userAccount.email);
            if(_admin != null)
            {
                model.accountPermissionID = _admin.accountPermissionID;
                model.userPermissionType = _admin.Permission.name;
            }
           
            model.adminUser = true;
            model.firstName = userAccount.firstName;
            model.lastName = userAccount.lastName;
            model.email = userAccount.email;
            model.dateAdded = userAccount.dateCreated;
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;

            return View(model);

        }

        //lists out the accounts of the site users on the system
        public ActionResult UserAccounts()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents non admin users from viewing the page
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("This page is restricted to super admin users.");
            }

            //calls method in repository that lists out all the accounts in the system
            IEnumerable<Account> accounts = accountDAO.FetchAllUserAccounts();
            
            //returns a list of only non admin accounts
            List<Account> userAccounts = new List<Account>();
            foreach (Account a in accounts)
            {
                var adminAccount = accountPermissionDAO.FetchByEmail(a.email);
                if(adminAccount == null )
                {
                    userAccounts.Add(a);
                }
            }
             
            List<Account> result = userAccounts.ToList();
              //wraps list into model
                    BeautySNS.Admin.Models.Accounts.IndexViewModel model = new BeautySNS.Admin.Models.Accounts.IndexViewModel(result);

                    model.adminUser = true;
                    model.userSession = userSession.LoggedIn;
                    model.loggedInAccount = account;
                    model.loggedInAccountID = account.accountID;
                    model.permissionType = adminUser.Permission.name;
                    return View(model);
        }


        public ActionResult UserDetails(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents access from non-admin users
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("This page is restricted to super admin users.");
            }

            //shows error message if the user does not exist
            Profile profile = profileDAO.fetchByAccountID(id);
            if (profile == null)
            {
                TempData["errorMessage"] = "This user does not exist";
                return RedirectToAction("SiteActivity","Alert");
            }

            BeautySNS.Admin.Models.Profiles.DetailsViewModel model = new BeautySNS.Admin.Models.Profiles.DetailsViewModel(profile);

            model.adminUser = true;
            model.userSession = userSession.LoggedIn;
            model.LoggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;

            return View(model);
        }


        //Get and post methods for deleting a user
        [HttpGet]
        public ActionResult DeleteUser(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents access from non admin users
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("This page is restricted to admin users.");
            }

            //shows error message if account doesn't exist
            Account _account = accountDAO.FetchById(id);
            if (_account == null)
            {
                TempData["errorMessage"] = "This user does not exist";
                return RedirectToAction("SiteActivity", "Alert");
            }
            BeautySNS.Admin.Models.Accounts.DeleteViewModel model = new BeautySNS.Admin.Models.Accounts.DeleteViewModel(_account);

            model.adminUser = true;
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.userSession = userSession.LoggedIn;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            return View(model);
        }

        [HttpPost, ActionName("DeleteUser")]
        public ActionResult DeleteConfirmed(int id)
        {
            Account _account = accountDAO.FetchById(id);
            accountDAO.Delete(id);
            alertService.AccountRemovedAlert(_account);

            if (id != null)
            {
                TempData["SuccessMessage"] = "Account was successfully deleted";
                return RedirectToAction("UserAccounts", "Admin");
            }

            else
            {
                TempData["errorMessage"] = "Error deleting Account";
            }

            return RedirectToAction("UserAccounts", "Admin");
        }

        //admin view of a user's network
        public ActionResult UserNetwork(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents access to non admin users
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("This page is restricted to admin users.");
            }

            //returns error message if user does not exist
            Account _account = accountDAO.FetchById(id);
            if(_account == null)
            {
                TempData["errorMessage"] = "This user does not exist";
                return RedirectToAction("SiteActivity", "Alert");
            }
            var friends = friendDAO.FetchFriendsAccountByAccountID(id);

            BeautySNS.Admin.Models.Accounts.IndexViewModel model = new BeautySNS.Admin.Models.Accounts.IndexViewModel(friends);

            model.adminUser = true;
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.userSession = userSession.LoggedIn;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            model.firstName = _account.firstName;
            model.userAccountID = _account.accountID;
            return View(model);
        }


    }
}