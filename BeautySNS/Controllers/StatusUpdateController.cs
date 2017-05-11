using BeautySNS.Admin.Models.StatusUpdates;
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
    public class StatusUpdateController : Controller
    {
        //initializes repositories that will be used in this controller
        private IStatusUpdateDAO statusUpdateDAO;
        private IUserSession userSession;
        private IProfileDAO profileDAO;
        private IAccountDAO accountDAO;
        private IAccountPermissionDAO accountPermissionDAO;
        private IAlertService alertService;

        public StatusUpdateController(IStatusUpdateDAO statusUpdateDAO, IUserSession userSession, IProfileDAO profileDAO, IAccountDAO accountDAO, IAccountPermissionDAO accountPermissionDAO, IAlertService alertService)
        {
            this.statusUpdateDAO = statusUpdateDAO;
            this.userSession = userSession;
            this.profileDAO = profileDAO;
            this.accountDAO = accountDAO;
            this.accountPermissionDAO = accountPermissionDAO;
            this.alertService = alertService;
        }

        //fetches the account of the logged in user
        public Account GetAccount()
        {
            return userSession.CurrentUser;
        }

        //fetches the profile of the logged in account
        public Profile GetProfile()
        {
            return profileDAO.fetchByAccountID(userSession.CurrentUser.accountID);
        }


        public ActionResult Index()
        {
            var statusUpdate = statusUpdateDAO.FetchAllStatusUpdates();           
            IndexViewModel model = new IndexViewModel(statusUpdate);
            return View(model);
        }

        //create status update
        [HttpGet]
        public ActionResult WriteStatusUpdate()
        {
            //prevents user from writing a status update if they are not logged in
            if(userSession.LoggedIn == false)
            {
                return Content("You have are not logged in ! Please login to write a status update.");
            }

            CreateViewModel model = new CreateViewModel();
            
            if(userSession.LoggedIn == true)
            {
                model.userSession = true;
            }

            else if (userSession.LoggedIn == false)
            {
               model.userSession = false;
            }

            //returns a partial view of the status update form
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult WriteStatusUpdate(CreateViewModel model)
        {
            //fetches account of logged in user and assigns the status update to their accountID
            Account account = GetAccount();

            if (ModelState.IsValid)
            {
                StatusUpdate statusUpdate = new StatusUpdate
                {
                    accountID = account.accountID,
                    status = model.status,
                    createDate = DateTime.Now,
                };
                statusUpdateDAO.CreateStatusUpdate(statusUpdate);
            }

            //redirects user to appropriate page after the status update has been submitted
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                return RedirectToAction("ProfileHomepage", "Profile");
            }

            else if (adminUser != null)
            {
                return RedirectToAction("MyActivity", "Admin");
            }

            return View();
        }

        //fetches the status updates of the logged in user
        public ActionResult MyStatusUpdates()
        {
            //prevents user from access their updates if they are not logged in
            if(userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to access your status updates.");
            }

            //fetches account of the logged in user and returns their status updates.
            Account account = GetAccount();
            List<StatusUpdate> statusUpdates = statusUpdateDAO.FetchStatusUpdatesByAccountID(account.accountID);
            return PartialView(statusUpdates);
        }


        /* A user's status updates shown when other people are viewing the user's profile.
           This view prevents another user from deleting a user's status update*/
        public ActionResult PublicStatusUpdates(int id = 0)
        {
            //prevents user from accessing their updates if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to access status updates.");
            }
           
            /* 
             Fetches account passed in to the browser and fetches the updates for that account.
             * If the account doesn't exist an error message is shown to the user and the user is
             * redirected back to their homepage based on the type of user they are i.e admin, non-admin etc..
             */         
            Account _account = GetAccount();
            Account account = accountDAO.FetchById(id);
            if (account == null)
            {
                var adminUser = accountPermissionDAO.FetchByEmail(_account.email);
                if(adminUser != null)
                {
                    TempData["errorMessage"] = "This user does not exist";
                    return RedirectToAction("SiteActivity", "Alert");                   
                }
                else if(adminUser == null)
                {
                    TempData["errorMessage"] = "This user does not exist";
                    return RedirectToAction("NewsFeed", "Alert"); 
                }
                
            }
            //lists out the status updates and returns it in a partial view.
            List<StatusUpdate> statusUpdates = statusUpdateDAO.FetchStatusUpdatesByAccountID(account.accountID);
            return PartialView(statusUpdates);
        }

        public ActionResult AllAccessStatusUpdates(int id = 0)
        {
            //prevents user from accessing their updates if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to access status updates.");
            }

            //prevents non admin users from viewing all access status updates
           Account _account = GetAccount();
            if (_account != null)
            {
                var adminUser = accountPermissionDAO.FetchByEmail(_account.email);
                if (adminUser == null)
                {
                    TempData["errorMessage"] = "Only admin users can view All Access status updates";
                    return RedirectToAction("NewsFeed", "Alert");
                }
                else if(adminUser != null)
                {
                    Account account = accountDAO.FetchById(id);
                    if(account == null)
                    {
                        TempData["errorMessage"] = "This user does not exist";
                        return RedirectToAction("SiteActivity", "Alert");
                    }
                    List<StatusUpdate> statusUpdates = statusUpdateDAO.FetchStatusUpdatesByAccountID(account.accountID);
                    return PartialView(statusUpdates);
                }
            }
            return View();
       }
            

        //delete status update
        public ActionResult Delete(int id = 0)
        {
            //prevents user from accessing their updates if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to access status updates.");
            }

            StatusUpdate status = statusUpdateDAO.FetchStatusUpdateByID(id);
            if (status == null)
            {
                return Content("Seems like that status update has already been deleted !");
            }
            Account accountBeingViewed = accountDAO.FetchById(status.accountID);

            Account account = GetAccount();
            if(account != null)
            {
                var admin = accountPermissionDAO.FetchByEmail(account.email);
                if(admin == null)
                {
                    TempData["errorMessage"] = "You can only delete your status updates";
                    return RedirectToAction("NewsFeed", "Alert");
                }
                else if(admin != null && admin.Permission.name == "Admin")
                {
                    var userAccount = accountPermissionDAO.FetchByEmail(accountBeingViewed.email);
                    {
                        if(userAccount != null)
                        {
                            TempData["errorMessage"] = "Only Super Admin users can delete other admin user's updates!";
                            return RedirectToAction("SiteActivity", "Alert");
                        }
                    }

                }
            }
            statusUpdateDAO.DeleteStatusUpdate(id);
            alertService.StatusUpdateRemovedAlert(status);

            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            
            if(adminUser == null)
            {
               return RedirectToAction("ProfileHomepage", "Profile");
            }

            else if (adminUser != null)
            {
                TempData["successMessage"] = "Status Update has been deleted";
                return RedirectToAction("SiteActivity", "Alert");
            }

            return View();
        }
    }
}