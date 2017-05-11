using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO
{
    public class AlertDAO : IAlertDAO
    {
        //creates an instance of the database
        private readonly BSNSContext _db;
        private IFriendDAO friendDAO;
        private IUserSession userSession;
        Alert alert = new Alert();

        public AlertDAO(BSNSContext db, IFriendDAO friendDAO, IUserSession userSession)
        {
            _db = db;
            this.friendDAO = friendDAO;
            this.userSession = userSession;
            //this.accountDAO = accountDAO;
        }

        //fetches an account by its id
        public Account FetchAccountById(int id)
        {
            return _db.Accounts.FirstOrDefault(a => a.accountID == id);
        }


        //fetch all alerts
        public IEnumerable<Alert> FetchAllAlerts()
        {
            return _db.Alerts.ToList();
        }

        public Alert FetchById(int id)
        {
            return _db.Alerts.FirstOrDefault(a => a.accountID == id);
        }


        //fetch alerts related to an account
        public List<Alert> FetchAlertsByAccountID(int accountID)
        {
            List<Alert> result = new List<Alert>();
            IEnumerable<Alert> alerts = _db.Alerts.Where(a => a.accountID == accountID)
                                                  .OrderByDescending(x => x.createDate).Take(40).ToList();
            result = alerts.ToList();
            return result;
        }

        public List<Alert> FetchAlertsByAlertType(int alertTypeID1, int alertTypeID2,int alertTypeID3,int alertTypeID4)
        {
            List<Alert> result = new List<Alert>();
            IEnumerable<Alert> alerts = _db.Alerts.Where(a => a.alertTypeID == alertTypeID1 || a.alertTypeID == alertTypeID2 || a.alertTypeID == alertTypeID3 || a.alertTypeID == alertTypeID4)
                                                  .OrderByDescending(x => x.createDate).Take(40).ToList();
            result = alerts.ToList();
            return result;
        }

        //create an alert
        public void CreateAlert(Alert alert)
        {
                alert.createDate = DateTime.Now;
                _db.Alerts.Add(alert);
                _db.SaveChanges();
        }

        //delete an alert
        public void DeleteAlert(int id)
        {
            Alert alert = _db.Alerts.Find(id);
            _db.Alerts.Remove(alert);
            _db.SaveChanges();
        }



    }
}