using BeautySNS.Admin.Models.Permissions;
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
    public class PermissionController : Controller
    {
        //initializes the repositories that will be used in this controller
        private IAccountPermissionDAO accountPermissionDAO;
        private IAccountDAO accountDAO;
        private IUserSession userSession;
        private IAlertService alertService;
        private IProfileDAO profileDAO;

        public PermissionController(IAccountPermissionDAO accountPermissionDAO, IUserSession userSession, IAccountDAO accountDAO,
                                     IAlertService alertService, IProfileDAO profileDAO)
        {
            this.accountPermissionDAO = accountPermissionDAO;
            this.accountDAO = accountDAO;
            this.userSession = userSession;
            this.alertService = alertService;
            this.profileDAO = profileDAO;
        }

        //shows all permissions
        public ActionResult PermissionIndex()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents access from users that are not super admin
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if(adminUser == null)
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if(adminUser.Permission.name != "SuperAdmin")
            {
                    return Content("Only Super Admin users are allowd to view this page");
            }

            var permission = accountPermissionDAO.FetchAllPermissions();
            PermissionIndexViewModel model = new PermissionIndexViewModel(permission);

            model.adminUser = true;
            if(userSession.LoggedIn == true)
            {

                model.userSession = true;
            }

            else if (userSession.LoggedIn != true)
            {
                model.userSession = false;
            }

            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            return View(model);

        }

        // Get and post methods for creating a permission
        [HttpGet]
        public ActionResult CreatePermission()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents users from accessing this functionality if they are not super admin
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if (adminUser.Permission.name != "SuperAdmin")
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            PermissionCreateViewModel model = new PermissionCreateViewModel();
            model.adminUser = true;
            if (userSession.LoggedIn == true)
            {

                model.userSession = true;
            }

            else if (userSession.LoggedIn != true)
            {
                model.userSession = false;
            }
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreatePermission(PermissionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Permission permission = new Permission
                {
                    name = model.name
                };

                //prevents user from creating a permission that already exists 
                var existingPermission = accountPermissionDAO.FetchPermissionByName(model.name);
                if(existingPermission != null)
                {
                    TempData["errorMessage"] = "This permission already exists";
                    return RedirectToAction("PermissionIndex");
                }

                else if (existingPermission == null)
                {
                    accountPermissionDAO.CreatePermission(permission);
                    alertService.AddPermissionCreatedAlert(permission);

                    if (permission != null)
                    {
                        TempData["SuccessMessage"] = "Permission was successfully created";
                    }

                    else
                    {
                        TempData["errorMessage"] = "Error saving Permission";
                    }

                    return RedirectToAction("PermissionIndex");
                }
            }

            return View(model);
        }


        //shows all account permissions
        public ActionResult AccountPermissionIndex()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //allows only admin users to access this method
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if (adminUser.Permission.name != "SuperAdmin")
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            var accountPermission = accountPermissionDAO.FetchAllAccountPermissions();
            AccountPermissionIndexViewModel model = new AccountPermissionIndexViewModel(accountPermission);

            model.adminUser = true;


            if (userSession.LoggedIn == true)
            {

                model.userSession = true;
            }

            else if (userSession.LoggedIn != true)
            {
                model.userSession = false;
            }

            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            return View(model);
        }


        //get and set methods for creating account permissions
        [HttpGet]
        public ActionResult CreateAccountPermission()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //allows only admin users to access this method
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if (adminUser.Permission.name != "SuperAdmin")
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            AccountPermissionCreateViewModel model = new AccountPermissionCreateViewModel(accountPermissionDAO.FetchAllPermissions());
            model.Permissions = accountPermissionDAO.FetchAllPermissions();

            if (userSession.LoggedIn == true)
            {

                model.userSession = true;
            }

            else if (userSession.LoggedIn != true)
            {
                model.userSession = false;
            }

            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            model.adminUser = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAccountPermission (AccountPermissionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                    AccountPermission accountPermission = new AccountPermission()
                    {
                        permissionID = model.permissionID,
                        email = model.email,
                        createDate = DateTime.Now,
                    };

                //admin permissions cannot be added to an account that does not exist on the system
                    var existingMember = accountDAO.FetchByEmail(model.email);
                    if(existingMember == null)
                    {
                        TempData["errorMessage"] = "This user does not exist in the system";
                        return RedirectToAction("AccountPermissionIndex");
                    }

                //admin permissions cannot be given to a user who has a profile
                    if (existingMember != null)
                    {
                        var profile =profileDAO.fetchByAccountID(existingMember.accountID);
                        if (profile != null)
                        {
                            TempData["errorMessage"] = "This user is a site member. Site members cannot be admin also !.";
                            return RedirectToAction("AccountPermissionIndex");
                        }
                        else if (profile == null)
                        {
                            //admin permissions cannot be given to a user who is already admin
                            var existingAdmin = accountPermissionDAO.FetchByEmail(model.email);
                            if(existingAdmin != null)
                            {
                                TempData["errorMessage"] = "This user is already admin. You can change their permission in Admin Users/Change Permission !";
                                return RedirectToAction("AccountPermissionIndex");
                            }
                               
                            else if (existingAdmin == null)
                            {
                                //adds the admin user to the database
                                accountPermission.accountID = existingMember.accountID;
                                accountPermissionDAO.CreateAccountPermission(accountPermission);
                                alertService.AddAdminUserCreatedAlert(accountPermission); //creates alert for admin news feed
                                TempData["successMessage"] = "Success. You have created a new admin user !";
                                return RedirectToAction("AccountPermissionIndex");
                            }
                        }

                    }                                               
            }
            model.Permissions = accountPermissionDAO.FetchAllPermissions();
            model.adminUser = true;
            return View(model);
        }

//shows the details of an admin user
        public ActionResult AccountPermissionDetails(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents users from accessing this methos if they are not super admin
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if (adminUser.Permission.name != "SuperAdmin")
            {
                return Content("Only Super Admin users are permitted to view this page");
            }


            AccountPermission accountPermission = accountPermissionDAO.FetchAccountPermissionByID(id);
            if (accountPermission == null)
            {
                TempData["errorMessage"] = "Sorry. That admin user does not exist !";
                return RedirectToAction("AccountPermissionIndex");
                //return Content("Sorry. This admin user doesn not exist");
            }

            AccountPermissionDetailsViewModel model = new AccountPermissionDetailsViewModel(accountPermission);


            if (userSession.LoggedIn == true)
            {

                model.userSession = true;
            }

            else if (userSession.LoggedIn != true)
            {
                model.userSession = false;
            }

            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            model.adminUser = true;
            return View(model);
        }


        //get and post methods for editing an admin user's permission
        [HttpGet]
        public ActionResult EditAccountPermission(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents users from accessing this method if they are not super admin
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);

            AccountPermission accountPermission = accountPermissionDAO.FetchAccountPermissionByID(id);
            if (adminUser == null)
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if (adminUser.Permission.name != "SuperAdmin")
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if (accountPermission == null)
            {
                TempData["errorMessage"] = "Sorry. That admin user does not exist !";
                return RedirectToAction("AccountPermissionIndex");
            }

            AccountPermissionEditViewModel model = new AccountPermissionEditViewModel(accountPermission, accountPermissionDAO.FetchAllPermissions());
            model.Permissions = accountPermissionDAO.FetchAllPermissions();

            if (userSession.LoggedIn == true)
            {

                model.userSession = true;
            }

            else if (userSession.LoggedIn != true)
            {
                model.userSession = false;
            }


            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            model.adminUser = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditAccountPermission(AccountPermissionEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                AccountPermission _accountPermission = accountPermissionDAO.FetchAccountPermissionByID(model.accountPermissionID);
                var user = accountPermissionDAO.FetchAccountPermissionByID(model.accountPermissionID);

                AccountPermission accountPermission = new AccountPermission
                {
                    accountPermissionID = model.accountPermissionID,
                    accountID = model.accountID,
                    permissionID = model.permissionID,
                    email = _accountPermission.email,
                    lastUpdateDate = DateTime.Now
                };

                //prevents user from changing the permission of a super admin user
                if (user.Permission.name == "SuperAdmin")
                {
                    TempData["errorMessage"] = "SuperAdmin users cannot be changed. Please see System Administrator !";
                    return RedirectToAction("AccountPermissionIndex");
                }
                else if (user.Permission.name != "SuperAdmin")
                {
                    accountPermissionDAO.updateAccountPermission(accountPermission);
                    alertService.AdminUpdatedAlert(accountPermission);
                    return RedirectToAction("AccountPermissionDetails", new { id = accountPermission.accountPermissionID });
                }
                
            }
            model.Permissions = accountPermissionDAO.FetchAllPermissions();
            model.userSession = userSession.LoggedIn;
            model.adminUser = true;
            return View(model);

        }

        //Get and post methods for deleting an account permission
        public ActionResult DeleteAccountPermission(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to view this page");
            }

            //prevents users who are not super admin from accessing this page
            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            if (adminUser.Permission.name != "SuperAdmin")
            {
                return Content("Only Super Admin users are permitted to view this page");
            }

            //returns error message if user fetches an account permission that does not exist
            AccountPermission accountPermission = accountPermissionDAO.FetchAccountPermissionByID(id);
            if (accountPermission == null)
            {
                TempData["errorMessage"] = "This admin user does not exist!";
                return RedirectToAction("AccountPermissionIndex");
            }

            //prevents user from deleting the permission of a super admin user
            if(accountPermission.Permission.name == "SuperAdmin")
            {
                TempData["errorMessage"] = "SuperAdmin users cannot be deleted. See System Administrator !";
                return RedirectToAction("AccountPermissionIndex");
            }

            AccountPermissionDeleteViewModel model = new AccountPermissionDeleteViewModel(accountPermission);
            model.email = accountPermission.email;
            model.Permission = accountPermission.Permission;
            model.permissionID = accountPermission.permissionID;
            model.createDate = accountPermission.createDate;
            model.lastUpdateDate = accountPermission.lastUpdateDate;
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            model.adminUser = true;
            return View(model);
        }

        [HttpPost, ActionName("DeleteAccountPermission")]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountPermission accountPermission = accountPermissionDAO.FetchAccountPermissionByID(id);
            accountPermissionDAO.DeleteAccountPermission(id);
            alertService.AdminUserRemovedAlert(accountPermission);

            if (id != null)
            {
                TempData["SuccessMessage"] = "Account Permission was successfully deleted";
                return RedirectToAction("AccountPermissionIndex");
            }

            else
            {
                TempData["errorMessage"] = "Error deleting Account Permission";
            }

            return RedirectToAction("AccountPermissionIndex");
        }

    }
}