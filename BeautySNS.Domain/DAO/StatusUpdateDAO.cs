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
    public class StatusUpdateDAO : IStatusUpdateDAO
    {
        //creates an instance of the database
        private readonly BSNSContext _db;
        private IAlertService alertService;

        public StatusUpdateDAO(BSNSContext db, IAlertService alertService)
        {
            _db = db;
            this.alertService = alertService;
        }

        //create a status update 
        public void CreateStatusUpdate(StatusUpdate statusUpdate)
        {
            _db.StatusUpdates.Add(statusUpdate);
             statusUpdate.createDate = DateTime.Now;
             alertService.AddStatusUpdateAlert(statusUpdate);
            _db.SaveChanges();
        }
      
        //fetch all status updates
        public IEnumerable<StatusUpdate> FetchAllStatusUpdates()
        {
            return _db.StatusUpdates.ToList();
        }

        //fetch status update by id
        public StatusUpdate FetchStatusUpdateByID(int statusUpdateID)
        {
            StatusUpdate result = _db.StatusUpdates.Where(su => su.statusUpdateID == statusUpdateID).FirstOrDefault();
            return result;
        }

        //fetch status update by account id
        public List<StatusUpdate> FetchStatusUpdatesByAccountID(int accountID)
        {
            List<StatusUpdate> result = new List<StatusUpdate>();
            IEnumerable<StatusUpdate> statusUpdates = _db.StatusUpdates.Where(su => su.accountID == accountID)
                                                                       .OrderByDescending(su => su.createDate);
            result = statusUpdates.ToList();
            return result;
     
        }
              
        //delete status update
         public void DeleteStatusUpdate(int id)
         {
             StatusUpdate statusUpdate = _db.StatusUpdates.Find(id);
             _db.StatusUpdates.Remove(statusUpdate);
             _db.SaveChanges();
         }

    }
}
