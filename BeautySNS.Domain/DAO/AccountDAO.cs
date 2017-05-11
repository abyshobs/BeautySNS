using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO
{
    public class AccountDAO : IAccountDAO
    {
        //creates an instance of the database
        private readonly BSNSContext _db;
        private IAlertService alertService;

        public AccountDAO(BSNSContext db, IAlertService alertService)
        {
            _db = db;
            this.alertService = alertService;
        }

        //creates an account and adds it to the database
        public void CreateAccount(Account account)
        {
            _db.Accounts.Add(account);
            _db.SaveChanges();
             //alertService.AddAccountCreatedAlert(); 

        }

        //updates an existing account
        public void Update(Account account)
        {
            Account originalAccount = _db.Accounts.Find(account.accountID);
            originalAccount.accountID = account.accountID;
            originalAccount.firstName = account.firstName;
            originalAccount.lastName = account.lastName;
            originalAccount.birthDate = originalAccount.birthDate;
            originalAccount.dateUpdated = DateTime.Now;
            originalAccount.password = account.password;
            originalAccount.email = account.email;
            _db.SaveChanges();
            alertService.AddAccountModifiedAlert(account);
        }



        //fetches an account by its id
        public Account FetchById(int id)
        {
            return _db.Accounts.FirstOrDefault(a => a.accountID == id);
        }

        //fetches account by email
        public Account FetchByEmail(string email)
        {
            return _db.Accounts.FirstOrDefault(e => e.email == email);
        }

        //Remove an account
        public void Delete(int id)
        {
            Account account = _db.Accounts.Find(id);
            if (account.Profile != null)
            {
                _db.Profiles.Remove(account.Profile);
                _db.Accounts.Remove(account);
                _db.SaveChanges();

            }
            else if (account.Profile == null)
            {
                _db.Accounts.Remove(account);
                _db.SaveChanges();
            }
            
        }

        public List<Account> SearchAccounts(string searchString)
        {
            List<Account> result = new List<Account>();
            IEnumerable<Account> accounts = from a in _db.Accounts
                                            where (a.firstName + "" + a.lastName).Contains(searchString) ||
                                                   a.email.Contains(searchString) ||
                                                   a.Profile.Job.name.Contains(searchString)
                                            select a;
            result = accounts.ToList();
            return result;

        }

        public IEnumerable<Account> FetchAllUserAccounts()
        {
            return _db.Accounts.ToList();
           
        }
    }
}
