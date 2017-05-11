using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IAlertDAO
    {
         IEnumerable<Alert> FetchAllAlerts();
         List<Alert> FetchAlertsByAccountID(int accountID);
         void CreateAlert(Alert alert);
         void DeleteAlert(int id);
         List<Alert> FetchAlertsByAlertType(int alertTypeID1, int alertTypeID2, int alertTypeID3, int alertTypeID4);
         Alert FetchById(int id);
         Account FetchAccountById(int id);
       
    }
}
