using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.Code;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Domain.Services.Interfaces;
using BeautySNS.Admin.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeautySNS.Admin.Controllers
{
    public class AccountController : Controller
    {
        //initializes the repositories that will be used by this controller
        private IAccountDAO accountDAO;
        private IProfileDAO profileDAO;
        private IAccountPermissionDAO accountPermissionDAO;
        private IAccountService accountService;
        private IEmail emails;
        private IConfiguration configuration;
        private IAlertService alertService;
        public ISessionWrapper sessionWrapper;
        public IUserSession userSession;


        public AccountController(IAccountService accountService, IAccountDAO accountDAO, IEmail emails, IConfiguration configuration, ISessionWrapper sessionWrapper, IUserSession userSession, IProfileDAO profileDAO, IAccountPermissionDAO accountPermissionDAO, IAlertService alertService)
        {
            this.accountService = accountService;
            this.accountDAO = accountDAO;
            this.emails = emails;
            this.configuration = configuration;
            this.sessionWrapper = sessionWrapper;
            this.userSession = userSession;
            this.profileDAO = profileDAO;
            this.accountPermissionDAO = accountPermissionDAO;
            this.alertService = alertService;
        }

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

        public ActionResult Index (string searchString)
        {
            //prevents users from accessing the page if they are not logged in
                if (userSession.LoggedIn == false)
                {
                    return Content("You are not logged in ! Please login to view this page");
                }

            //prevents user from searching a profile if they haven't created their profile or if they are admin
               Account account = userSession.CurrentUser;
               var adminUser = accountPermissionDAO.FetchByEmail(account.email);

              if(adminUser != null)
              {
                return Content("Please use the search engine in the admin site");
              }
     
               if(account.Profile == null)
               {              
                   TempData["errorMessage"] = "This search isn't available to users without a BeautySNS profile !";
                   return RedirectToAction("Create", "Profile");
                }

               else if(account.Profile != null)
               {
                   List<Account> accounts = accountDAO.SearchAccounts(searchString);
                   if (accounts.Count == 0)
                    {
                      TempData["errorMessage"] = "No search results !";
                      return RedirectToAction("NewsFeed", "Alert");
                       //return RedirectToAction("Index");
                    }

                    else if (accounts.Count > 0)
                    {
                      //wraps the list of accounts into the index model
                      IndexViewModel model = new IndexViewModel(accounts);
                      model.userSession = userSession.LoggedIn;
                      model.loggedInAccount = account;
                      model.loggedInAccountID = account.accountID;
                      model.fullName = string.Format("{0} {1}", model.firstName, model.lastName);
                      model.adminUser = false;  
                      return View(model);
                    }
               }
               
            return View();
        }
       
        

        //gets the partial login form
        [AllowAnonymous]
        //[HttpGet] 
        public ActionResult LoginPartial(string email, string password)
        {
            LoginViewModel model = new LoginViewModel();

            //prevents user from accessing this method if they are logged in
            Account account = GetAccount();
            if(account !=null && userSession.LoggedIn == true)
            {
                return HttpNotFound();
            }
           
            if (account == null)
            {
                model.userSession = false;
            }

            ViewBag.email = email;
            ViewBag.password = password;
            return PartialView();
        }

        //gets the login form
        [AllowAnonymous]
        public ActionResult Login(string email, string password)
        {
            LoginViewModel model = new LoginViewModel();

            //prevents user from logging in if they are already logged in
            Account account = GetAccount();
            if (account != null)
            {
                var adminUser = accountPermissionDAO.FetchByEmail(account.email);
                if (account != null && adminUser == null)
                {
                    return RedirectToAction("NewsFeed", "Alert");
                }

                else if (account != null && adminUser != null)
                {
                    return RedirectToAction("SiteActivity", "Alert");
                }
            }

            if (account == null)
            {
                model.userSession = false;
            }

            ViewBag.email = email;
            ViewBag.password = password;
            return View(model);
        }

        //post method for login 
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string email, string password)
        {
            model.userSession = false;
            if (ModelState.IsValid)
            {
                email = model.email;
                password = model.password.Encrypt(email);
                Account account = accountDAO.FetchByEmail(email);
                var adminUser = accountPermissionDAO.FetchByEmail(email);

                //if there is only one account returned - good
                if (account != null)
                {
                    //password matches
                    if (account.password == password)
                    {
                        if (account.emailVerified)
                        {
                            userSession.LoggedIn = true;
                            userSession.Email = email;
                            userSession.CurrentUser = accountDAO.FetchById(account.accountID);

                            //redirects users to their appropriate pages
                            if (adminUser != null)
                            {
                                return RedirectToAction("SiteActivity", "Alert");
                            }

                            else if (adminUser == null)
                            {

                                var profile = profileDAO.fetchByAccountID(userSession.CurrentUser.accountID);
                                if (profile != null)
                                    return RedirectToAction("NewsFeed", "Alert");
                                else
                                    return RedirectToAction("Create", "Profile");
                            }
                        }
                            //if user attempts to login without verifying theiremail account
                        else
                        {
                            emails.SendEmailAddressVerificationEmail(account.email, account.email);
                            TempData["errorMessage"] = @"The login information you provided was correct 
                                but your email address has not yet been verified.  
                                We just sent another email verification email to you.  
                                Please follow the instructions in that email.";
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = @"We were unable to log you in with that information!";
                        return RedirectToAction("Login", "Account");
                    }
                }

                TempData["errorMessage"] = @"We were unable to log you in with that information!";
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        //logs user out of the system
        public ActionResult Logout()
        {
            userSession.LoggedIn = false;           
            return RedirectToAction("Register");         
        }

        //get the register form
        [HttpGet]
        public ActionResult Register()
        {
            //logs user out if they attempt to register while they are logged in 
            Account account = GetAccount();
            if(account != null)
            {
                userSession.LoggedIn = false;
            }

            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }


        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            model.userSession = false;

            //creates a new account if the form has passed validation
            if (ModelState.IsValid)
            {
                Account account = new Account
                 {
                     firstName = model.firstName,
                     lastName = model.lastName,
                     email = model.email,
                     password = model.password.Encrypt(model.email), //encrypts the user's password
                     birthDate = model.birthDate,
                     dateCreated = DateTime.Now,

                 };

                //returns error message if the email is already in use
                if (accountService.EmailInUse(account.email))
                {
                    return Content("That email is already being used. Please try again with another email");
                }
                 
                //otherwise creates an account and sends verification email
                else
                {
                    model.userSession = false;
                    accountDAO.CreateAccount(account);
                    emails.SendEmailAddressVerificationEmail(account.email, account.email);
                    TempData["successMessage"] = "You have created an account ! A confirmation has been sent to your email";
                    return RedirectToAction("EmailConfirmation", "Account");
                }              
            }
            return View(model);
        }

        //user confirmation when they create an account
        public ActionResult EmailConfirmation()
        {
            LoginViewModel model = new LoginViewModel();
            model.userSession = false;
            return View(model);
        }


        //uses link from email to verify the email
        public ActionResult VerifyEmail()
        {
            LoginViewModel model = new LoginViewModel();
            model.userSession = false;
          
            //fetches the email being verified and fetches the account associated with the email
            string email = Cryptography.Decrypt(sessionWrapper.EmailToVerify, "verify");                      
            Account accounts = accountDAO.FetchByEmail(email);

            //updates the account 
            if (accounts != null)
            {
                model.userSession = false;
                accounts.emailVerified = true;
                accountDAO.Update(accounts);
                TempData["successMessage"] = "Account has been verified ! ";
                return RedirectToAction("Login");
            }

            //shows error message if account can't be updated
            else
            {
                model.userSession = false;
                TempData["errorMessage"] = "We couldn't verify your account ! ";
                return RedirectToAction("Login");
            }

        }

        //gets form which allows users to recover lost passwords
        [HttpGet]
        public ActionResult RecoverPassword()
        {
            Account account = GetAccount();
            if(account != null)
            {
                return Content("You are logged in ! Please logout to View this page");
            }

            RecoverPasswordViewModel model = new RecoverPasswordViewModel();
            
            if(userSession.LoggedIn == false)
            {
                model.userSession = false;
            }
           
            return View(model);
        }

    
        [HttpPost]
        public ActionResult RecoverPassword(RecoverPasswordViewModel model, string email)
        {
            if (ModelState.IsValid)
            {
                //sends a password recovery email if the account exists in the system
                email = model.email;
                Account account = accountDAO.FetchByEmail(email);
                if (account != null)
                {
                    if (account.emailVerified)
                    {
                        emails.PasswordRecoveryEmail(account.email, account.email);
                        TempData["successMessage"] = "An email has been sent to you!";
                        return RedirectToAction("Recoverpassword", "Account");
                    }
                    else 
                    {
                        emails.SendEmailAddressVerificationEmail(account.email, account.email);
                        TempData["errorMessage"] = @"The email you provided was correct 
                                but your email address has not yet been verified.  
                                We just sent another email verification email to you.  
                                Please follow the instructions in that email.";   
                    }
                }
                else
                {
                    TempData["errorMessage"] = @"We could not find that email !
                                                 contact customer service on beautysnsuk@gmail.com 
                                                 if you need more help";
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            Account account = GetAccount();
            if(account != null)
            {
                return Content("Sorry ! You can't use this feature while logged in");
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.userSession = false;

            //decrypts the email from the password recovery link and fetches the account associated with the email
            string email = Cryptography.Decrypt(sessionWrapper.EmailToVerify, "verify");
            Account _account = accountDAO.FetchByEmail(email);
            model.email = _account.email;
            
            return View(model);
        }
      

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model, string password)
        {
            //fetches the account associated with the email obtaained in the GET method
            Account account = accountDAO.FetchByEmail(model.email);

            //updates the account if it exists
            if (account != null)
            {
                account.password = model.password.Encrypt(model.email);
                accountDAO.Update(account);
                TempData["successMessage"] = @"Your Password has been changed !";
                return RedirectToAction("Login", "Account");
            }

             //raises error message if the password cannot be changed
            else
            {
                TempData["errorMessage"] = @"We could not change your password.Contact customer service on beautysnsuk@gmail.com 
                                                         if you need more help ";
                return RedirectToAction("ChangePassword", "Account");
            }

        }

        //gets form which allows users to change their email
        [HttpGet]
        public ActionResult ChangeEmail()
        {
            //prevents users from accessing the form if they are not logged in
            Account account = GetAccount();
            if(account == null)
            {
                return Content("You are not logged in. Please login to change your email");
            }

            //prevents admin users from accessing this feature
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if(adminUser != null)
            {
                return Content("This feature is not available to admin users");
            }
            
            ChangeEmailViewModel model = new ChangeEmailViewModel();
            model.userSession = userSession.LoggedIn;
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            model.adminUser = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailViewModel model, string email)
        {
            //checks if email already exists in the system          
            var _email = accountDAO.FetchByEmail(email);

            if (_email == null)
            {
                emails.SendChangedEmailAddressVerificationEmail(model.email, model.email);
                TempData["successMessage"] = "A confirmation has been sent to your email. Your email will be changed once you follow the link in your email";
                return RedirectToAction("ChangeEmail", "Account");
            }
            
            //prevents user from changing their email to email that already exists in the system
            else if(_email != null)
            {
                TempData["errorMessage"] = "That email already exists in the system";
                return RedirectToAction("ChangeEmail", "Account");
            }

            return View(model);
        }

        //verifies the changed email
        public ActionResult VerifyChangedEmail()
        {
            userSession.LoggedIn = false;

            //fetches the email from link
            string email = Cryptography.Decrypt(sessionWrapper.EmailToVerify, "verify");
            Account accounts = userSession.CurrentUser;

            //updates the user account with the new email
            if (accounts != null)
            {
                accounts.password = accounts.password.Decrypt(accounts.email); //decrypts the existing password
                accounts.email = email;                
                accounts.password = accounts.password.Encrypt(accounts.email);//encrypts the exsisting password using the new email
                accountDAO.Update(accounts); //updates the account in the database
                return Content("Your email has been changed. Please login again to view changes");
            }

            //returns error message if email can't be verified
            else
            {
                return Content("we couldn't change that email");
            }

        }

        //fecthes the account details of the logged in user
        public ActionResult AccountDetails()
        {
            //prevents users that are not logged in and admin users from accessing this method
            Account account = GetAccount();
            if (account == null)
            {
                return Content("You are not logged in ! Please login to view your account details");
            }

            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if(adminUser != null)
            {
                TempData["errorMessage"] = "Super Admin users can view account details using Account Permission details.";
                return RedirectToAction("SiteActivity", "Alert");
            }

            //fetches the account and wraps it in the details model
            account = accountDAO.FetchById(account.accountID);
            DetailsViewModel model = new DetailsViewModel(account);

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
            model.adminUser = false;
            return View(model);
        }


        //Get and post method for updating account details
        [HttpGet]
        public ActionResult EditAccount(int id = 0)
        {
            /*prevents user from accessing the method if they are not logged in 
            or if it isn't their logged in account or if the logged in user is admin*/
            Account account = GetAccount();
            if (account == null)
            {
                return Content("You are not logged in. Please login to view this page");
            }

            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if(adminUser != null)
            {
                TempData["errorMessage"] = "You are not permitted to edit a user's account.";
                return RedirectToAction("SiteActivity", "Alert");
            }

            var _account = accountDAO.FetchById(id);

            if (account != null && _account != null)
            {
                if (_account.accountID != account.accountID)
                {
                    TempData["errorMessage"] = "You are not permitted to edit another user's account.";
                    return RedirectToAction("NewsFeed", "Alert");
                }
            }
            if (_account == null)
            {
                TempData["errorMessage"] = "That account doesn't exist";
                return RedirectToAction("NewsFeed", "Alert");
            }

            EditViewModel model = new EditViewModel(_account);
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;
            if(userSession.LoggedIn == true)
            {
                model.userSession = true;
            }

            else if (userSession.LoggedIn == false)
            {
                model.userSession = false;
            }
            
            model.adminUser = false;
            return View(model);
        }


        [HttpPost]
        public ActionResult EditAccount(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = userSession.CurrentUser;
                account.firstName = model.firstName;
                account.lastName = model.lastName;
                //account.birthDate = model.birthDate;              

                accountDAO.Update(account); //updates the account in the database

                if (account != null)
                {
                    TempData["SuccessMessage"] = "Account was successfully updated";
                }

                //raises error if account cannot be updated
                else
                {
                    TempData["errorMessage"] = "Error updating Account";
                }

                return RedirectToAction("AccountDetails", new { id = account.accountID });
            }

            return View(model);
        }

        //enables logged in user to change their password
        [HttpGet]
        public ActionResult EditPassword()
        {
            //prevents users from accessing this method if they are not logged in or if they are admin
            if(userSession.LoggedIn == false)
            {
                return Content("You are not logged in! Please login to view this page.");
            }

            Account account = userSession.CurrentUser;
            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if(adminUser != null)
            {
                TempData["errorMessage"] = "To change your password, please logout and click on 'Forgotten Your Password' on the homepage";
                return RedirectToAction("SiteActivity", "Alert");
            }

            EditPasswordViewModel model = new EditPasswordViewModel();

            if(userSession.LoggedIn == true)
            {
                model.userSession = true;
            }

            else if (userSession.LoggedIn == false)
            {
                model.userSession = false;
            }
            
            model.adminUser = false;
            model.loggedInAccount = account;
            model.loggedInAccountID = account.accountID;

            return View(model);
        }
        
        [HttpPost]
        public ActionResult EditPassword(EditPasswordViewModel model, string currentPassword, string newPassword)
        {
            //updates the password of the logged in user
            Account account = userSession.CurrentUser;

            currentPassword = model.currentPassword.Encrypt(account.email);

            if (account != null)
            {
                if (account.password == currentPassword)
                {
                    account.password = model.newPassword.Encrypt(account.email);
                    accountDAO.Update(account);
                    TempData["successMessage"] = "Your password has been changed !";
                }
                else
                {
                    TempData["errorMessage"] = "The current password is incorrect";
                }
            }

            else
            {
                TempData["errorMessage"] = "we could not update your password";
            }

            return View(model);
        
        }

        //Get and post methods for deleting a user's account
        [HttpGet]
        public ActionResult DeleteAccount (int id = 0)
        {
            //prevents users from accessing this method if they are not logged in
            if (userSession.LoggedIn == false)
            {
                return Content("You are not logged in! Please login to view this page.");
            }

            Account account = userSession.CurrentUser;
            if (account == null)
            {
                return Content("This account does not exist");
            }

            var adminUser = accountPermissionDAO.FetchByEmail(account.email);
            if (adminUser != null)
            {
                TempData["errorMessage"] = "To delete a user, please do so on 'User Accounts/ Delete User'";
                return RedirectToAction("SiteActivity", "Alert");
            }

            DeleteViewModel model = new DeleteViewModel(account);
           
            if (userSession.LoggedIn == true)
            {
                model.userSession = true;
            }

            else if (userSession.LoggedIn == false)
            {
                model.userSession = false;
            }

            model.loggedInAccount = account;
            model.adminUser = false;
            return View(model);
        }

        [HttpPost, ActionName("DeleteAccount")]
        public ActionResult DeleteConfirmed(int id)
        {
            accountDAO.Delete(id);

            if (id != null)
            {
                //TempData["SuccessMessage"] = "Account was successfully deleted";
                userSession.LoggedIn = false;
                return Content("Sorry you had to leave us. We hope to see you again soon !");
                //return RedirectToAction("Register", "Account");
            }

            else
            {
                TempData["errorMessage"] = "Error deleting Account";
            }

            return RedirectToAction("Index");
        }

        //gets images of users' avatars
        public FileContentResult getImg(int id = 0)
        {
                Account account = accountDAO.FetchById(id);
                byte[] byteArray = account.Profile.avatar;
                return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
            }


        }
    }
