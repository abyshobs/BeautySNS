using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IStatusUpdateDAO
    {
        void CreateStatusUpdate(StatusUpdate statusUpdate);
        IEnumerable<StatusUpdate> FetchAllStatusUpdates();
        StatusUpdate FetchStatusUpdateByID(int statusUpdateID);
        List<StatusUpdate> FetchStatusUpdatesByAccountID(int accountID);
        void DeleteStatusUpdate(int id);
      
    }
}
