using BeautySNS.Admin.Models.Jobs;
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
    public class JobController : Controller
    {
        //initializes the repositories that will be used in this controller
        private IJobDAO jobDAO;
        private IUserSession userSession;
        private IAccountPermissionDAO accountPermissionDAO;
        private IAlertService alertService;

        public JobController(IJobDAO jobDAO, IUserSession userSession, IAccountPermissionDAO accountPermissionDAO, IAlertService alertService)
        {
            this.jobDAO = jobDAO;
            this.userSession = userSession;
            this.accountPermissionDAO = accountPermissionDAO;
            this.alertService = alertService;
        }

        //fetches the account of the logged in user
        public Account GetAccount()
        {
            return userSession.CurrentUser;
        }

        //returns a list of the jobs
        public ActionResult Index()
        {
            //prevents user from accessing the page if they are not logged in
            if(userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please log in to view this page.");
            }

            //prevents users from accessing the page if they are not admin
            Account account = GetAccount();
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if(adminUser == null)
            {
                TempData["errorMessage"] = "This page is only available to admin users!";
                return RedirectToAction("NewsFeed", "Alert");
            }

            //returns an index of all the jobs in the system
            var job = jobDAO.FetchAll();
            IndexViewModel model = new IndexViewModel(job);

            if(userSession.LoggedIn == true)
            {
                model.userSession = true;
            }

            else if (userSession.LoggedIn == false)
            {
                model.userSession = false;
            }

            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.permissionType = adminUser.Permission.name;
            model.adminUser = true;

            return View(model);
        }


        // Get and post methods for creating a job
        [HttpGet]
        public ActionResult Create()
        {
            //prevents user from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please log in to view this page.");
            }

            //prevents users from accessing the page if they are not admin
            Account account = GetAccount();
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                TempData["errorMessage"] = "This page is only available to admin users!";
                return RedirectToAction("NewsFeed", "Alert");
            }

            CreateViewModel model = new CreateViewModel();
            if (userSession.LoggedIn == true)
            {
                model.userSession = true;
            }

            else if (userSession.LoggedIn == false)
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
        public ActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Job job = new Job
                {
                    name = model.name
                };

                //prevents user from creating a job that already exists in the system
                var existingJob = jobDAO.FetchByName(model.name);
                if(existingJob != null)
                {
                    TempData["errorMessage"] = "That Job already exists in the system";
                }
                
                 //adds the job if it doesn't already exist
                else if (existingJob == null)
                {
                    jobDAO.Create(job);
                    alertService.JobCreatedAlert(job);

                    if (job != null)
                    {
                        TempData["SuccessMessage"] = "Job was successfully created";
                    }

                    else
                    {
                        TempData["errorMessage"] = "Error saving job";
                    }
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        //Get and Post methods for updating a job
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            //prevents user from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please log in to view this page.");
            }

            //prevents users from accessing the page if they are not admin
            Account account = GetAccount();
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser == null)
            {
                TempData["errorMessage"] = "This page is only available to admin users!";
                return RedirectToAction("NewsFeed", "Alert");
            }


            var job = jobDAO.FetchById(id);
            if (job == null)
            {
                TempData["errorMessage"] = "That job does not exist!";
                return RedirectToAction("Index", "Job");
            }

            EditViewModel model = new EditViewModel(job);

            if (userSession.LoggedIn == true)
            {
                model.userSession = true;
            }

            else if (userSession.LoggedIn == false)
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
        public ActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Job job = new Job
                {
                    jobID = model.jobID,
                    name = model.name
                };

                //prevents user from editing a job to a job already exists in the system
                var existingJob = jobDAO.FetchByName(model.name);
                if (existingJob != null)
                {
                    TempData["errorMessage"] = "That Job already exists in the system";
                }

                 //edits the job if the job doesn't already exist
                else if (existingJob == null)
                {
                    jobDAO.Update(job);
                    alertService.JobUpdatedAlert(job);

                    if (job != null)
                    {
                        TempData["SuccessMessage"] = "Job was successfully edited";
                    }

                    else
                    {
                        TempData["errorMessage"] = "Error editing job";
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

      
    }
}