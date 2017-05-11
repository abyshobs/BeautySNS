using BeautySNS.Admin.Models.Profiles;
using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySNS.Controllers
{
    public class ProfileController : Controller
    { 
        //initializes the repositories that will be used in this controller

        private IFriendDAO friendDAO;
        private IFriendInvitationDAO friendInvitationDAO;
        private IAccountDAO accountDAO;
        private IProfileDAO profileDAO;
        private IJobDAO jobDAO;
        private IUserSession userSession;
        private Profile profile;
        private IAccountPermissionDAO accountPermissionDAO;
        private IAlertService alertService;

        public ProfileController() { }

        public ProfileController(IFriendDAO friendDAO, IFriendInvitationDAO friendInvitationDAO, IAccountDAO accountDAO, IProfileDAO profileDAO, IUserSession userSession, IJobDAO jobDAO, Profile profile, IAccountPermissionDAO accountPermissionDAO, IAlertService alertService)
        {
            this.friendDAO = friendDAO;
            this.friendInvitationDAO = friendInvitationDAO;
            this.accountDAO = accountDAO;
            this.profileDAO = profileDAO;
            this.userSession = userSession;
            this.jobDAO = jobDAO;
            this.profile = profile;
            this.accountPermissionDAO = accountPermissionDAO;
            this.alertService = alertService;
        }

        BeautySNS.Admin.Models.Accounts.LoginViewModel sessionModel = new BeautySNS.Admin.Models.Accounts.LoginViewModel();

        //fetches the account of the logged in user
        public Account GetAccount()
        {
            return userSession.CurrentUser;
        }

        //fetches the profile of the logged in user
        public Profile GetProfile()
        {
            return profileDAO.fetchByAccountID(userSession.CurrentUser.accountID);
        }

        //page that shows users a private view of their profile page
        public ActionResult ProfileHomePage()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please login to visit your dashboard");
            }

            //if the logged in account is no longer available user is directed to the homepage
            Account account = GetAccount();
            if(account == null)
            {
                return RedirectToAction("Register", "Account");
            }

            //prevents admin users from accessing the page
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser != null)
            {
                return Content("Admin Users are not permitted to view this page");
            }

            var profile = GetProfile();            
            if (profile == null)
            {
                return Content("Sorry this page does not exist !");
            }
               
            /*wraps the profile into its model and sets some of the values in the model.
            Other values are set in the view*/

            DetailsViewModel model = new DetailsViewModel(profile);
            model.adminUser = false;
            if (userSession.LoggedIn == true)
            {
                model.userSession = true;
            }
            else if(userSession.LoggedIn == false)
            {
                model.userSession = false;
            }
            model.loggedInAccountID = account.accountID;
            model.fullName = string.Format("{0} {1}",model.Account.firstName, model.Account.lastName);
            return View(model);
           
        }

        //This is the public profile page which other users are allowed to see
        public ActionResult UserProfileHomepage(int id = 0)
        {
            if(userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please Login to view this page");
            }

            //sets values that will be used
            bool adminUser = false;
            bool sameAccount = true;
            bool isFriend = false;
            bool isNotFriend = false;
            Profile profile = GetProfile();
            Account account = GetAccount();
            var _adminUser = accountPermissionDAO.FetchByEmail(account.email);
            
           
            Profile profileBeingViewed = profileDAO.fetchByAccountID(id);
            if (profileBeingViewed == null)
            {
                return Content("Sorry ! This profile does not exist");
            }
           
            DetailsViewModel model = new DetailsViewModel(profileBeingViewed);
            model.userSession = userSession.LoggedIn;
            model.fullName = string.Format("{0} {1}", model.Account.firstName, model.Account.lastName);

            //sets values in the model based on conditions
            if (_adminUser != null)
            {
                adminUser = true;
                model.adminUser = adminUser;
            }

            else if (_adminUser == null)
            {
                var account1 = profileBeingViewed.Account;
                var account2 = profile.Account;

                if (account1 == account2)
                {
                    sameAccount = true;
                    model.sameAccount = sameAccount;
                }

                if (friendDAO.IsFriend(account1, account2))
                {
                    isFriend = true;
                    model.isFriend = isFriend;
                }

                if (friendDAO.IsFriend(account2, account1))
                {
                    isFriend = true;
                    model.isFriend = isFriend;
                }

                if (!friendDAO.IsFriend(account1, account2))
                {
                    isNotFriend = true;
                    model.isNotFriend = isNotFriend;
                }
            }
            return View(model);
        }

        //fetches the profile details of a user
        public ActionResult ProfileDetails(int id = 0)
        {
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please Login to view this page");
            }

            Account account = GetAccount();
            Account _account = accountDAO.FetchById(id); //fetches user account by its accountID based on the id passed into this method.
            
            //An error message is shown if a profile does not exist for the account
            Profile profile = profileDAO.fetchByAccountID(id);
            if (profile == null)
            {
                    return Content("Sorry that profile does not exist");
            }

            //Admin users have a different functionality in the admin controller for viewing user details.
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser != null)
            {
                TempData["errorMessage"] = "To view a user's details go to User Accounts/View details";
                return RedirectToAction("SiteActivity", "Alert");
                //return Content("To view a user's details go to User Accounts/View details");
            }

            DetailsViewModel model = new DetailsViewModel(profile);
            model.fullName = string.Format("{0} {1}", model.Account.firstName, model.Account.lastName);
            model.loggedInAccountID = account.accountID;
            model.LoggedInAccount = account;
            model.userSession = userSession.LoggedIn;
            model.userAccount = _account.accountID;
            model.adminUser = false;

            return View(model);
        }

        //gets the form that allows users to create a profile
        [HttpGet]
        public ActionResult Create()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please Login to create a profile");
            }

            //fetches the logged in user and checks if they already have a profile
            Account account = GetAccount();
            var _account = accountDAO.FetchById(account.accountID);
            if(_account.Profile != null)
            {
                TempData["errorMessage"] = "You already have a BeautySNS Profile!";
                return RedirectToAction("NewsFeed", "Alert");
            }

            //checks if the logged in user is admin
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if(adminUser != null)
            {
                TempData["errorMessage"] = "Sorry! Admin users are not permitted to create profiles";
                return RedirectToAction("SiteActivity", "Alert");
            }

            CreateViewModel model = new CreateViewModel(jobDAO.FetchAll());
            model.Jobs = jobDAO.FetchAll(); //fetches all the jobs a user can select from
            model.userSession = userSession.LoggedIn;
            model.adminUser = false;
            return View(model);
        }

        //post method for creating a profile
        [HttpPost]
        public ActionResult Create(CreateViewModel model)
        {         
            if (ModelState.IsValid)
            {
                Account account = GetAccount(); //fetches the logged in user
                Profile profile = GetProfile();
                  
                //if the logged in user does not have a profile already, a new profile is created.
                if (profile == null)
                    profile = new Profile()
                    {
                        accountID = account.accountID,
                        jobID = model.jobID,
                        aboutMe = model.aboutMe,
                        education = model.education,
                        experience = model.experience,
                        website = model.website,
                        location = model.location,
                        createDate = DateTime.Now,
                    };
               
                profileDAO.CreateProfile(profile);  //fetches the method in the repository which adds the new profile to the database
                alertService.AddProfileCreatedAlert(); //creates an alert for the news feeed that a new profile has been created
                return RedirectToAction("FileUpload", "Profile"); //allows user to upload a profile pic after creating a profile
            }
            model.Jobs = jobDAO.FetchAll(); 
            model.userSession = userSession.LoggedIn;
            return View(model);
        }


        //gets the form which allows users to edit their profile
        [HttpGet]
        public ActionResult EditProfile(int id = 0)
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please Login to access this page");
            }

            Account account = GetAccount();
            Profile profile = GetProfile();

            if (profile == null)
            {
                return Content("Access Denied.You can only edit your profile.");
            }

            //checks if the logged in user is admin
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser != null)
            {
                TempData["errorMessage"] = "Please do not attempt to edit someone else's details !";
                return RedirectToAction("SiteActivity", "Alert");
            }

            EditViewModel model = new EditViewModel(profile, jobDAO.FetchAll());
            model.Jobs = jobDAO.FetchAll();
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.adminUser = false;
            return View(model);
        }

        //method for posting the changed profile details
        [HttpPost]
        public ActionResult EditProfile(EditViewModel model)
        {
            //fetches the logged in user's profile
            Profile _profile = GetProfile();

            if (ModelState.IsValid)
            {
                //creates the new data and adds it to the profile
                Profile profile = new Profile
                {
                    accountID = model.accountID,
                    jobID = model.jobID,
                    Account = _profile.Account,
                    avatar = _profile.avatar,
                    education = model.education,
                    experience = model.experience,
                    website = model.website,
                    location = model.location,
                    aboutMe = model.aboutMe,
                    lastUpdateDate = DateTime.Now,                 
                    
                };
                profile.Account.firstName = model.firstName;
                profile.Account.lastName = model.lastName;

                profileDAO.UpdateProfile(profile); //fetches the method in the repository which adds the updated profile to the database
                return RedirectToAction("ProfileDetails", new { id = model.accountID }); //redirects to the profile details of the user
            }
            model.Jobs = jobDAO.FetchAll();
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = GetAccount();
            return View(model);
        }


        //gets the form which allows users to upload a profile picture
        [HttpGet]
        public ActionResult FileUpload()
        {
            //prevents users from accessing the page if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in ! Please Login to access this page");
            }

            //prevents admin users from trying to upload profile pictures
            Account account = GetAccount();
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser != null)
            {
                TempData["errorMessage"] = "Admin users do not need to upload profile pictures !";
                return RedirectToAction("SiteActivity", "Alert");
            }

            DetailsViewModel model = new DetailsViewModel();
            model.userSession = userSession.LoggedIn;
            return View(model);
        }

        //post method for posting the image to the database
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            Profile profile = GetProfile();
            if (file != null)
            {
                byte[] uploadedImage = new byte[file.InputStream.Length];
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/content/images/avatars"), pic);
                string extension = Path.GetExtension(file.FileName).ToLower();

                //ensures that only the correct file format is uploaded
                if (extension == ".png" )
                {
                    file.SaveAs(path);
                }
                else if(extension == ".jpg" )
                {
                    file.SaveAs(path);
                }
                else if (extension == ".gif")
                {
                    file.SaveAs(path);
                }
                // file is uploaded
                else
                {
                    TempData["errorMessage"] = "We only accept .png, .jpg, and .gif!";
                    return RedirectToAction("FileUpload", "Profile");
                }
                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                 if (file.ContentLength / 1000 < 1000)
                  {
                     using (MemoryStream ms = new MemoryStream())
                     {
                       file.InputStream.Read(uploadedImage, 0, uploadedImage.Length);
                       profile.avatar = uploadedImage;
                     }
                }

                 else 
                 {
                     TempData["errorMessage"] = @"The file you uploaded exceeds the size limit. 
                                              Please reduce the size of your file and try again.";
                 }
                }
            // after successfully uploading save changes to database
            profileDAO.UpdateProfile(profile);
            return RedirectToAction("ProfileHomepage");
        }
               
        //method for converting the byte in the database into an actual picture
        public FileContentResult getImg(int id = 0)
        {
            Account account = accountDAO.FetchById(id);
            Profile currentProfile = profileDAO.fetchByAccountID(account.accountID);

            if (currentProfile.Account.accountID == id)
            {
                byte[] byteArray = currentProfile.avatar;
                return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
            }
            
            if(account.accountID == id)
            {
                    byte[] byteArray = account.Profile.avatar;
                    return byteArray != null
                    ? new FileContentResult(byteArray, "image/jpeg")
                    : null;
            }

            else 
            {                
                Profile profile = profileDAO.fetchByAccountID(id);
                byte[] byteArray = profile.avatar;
                return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
            }
        }
    
   }
}