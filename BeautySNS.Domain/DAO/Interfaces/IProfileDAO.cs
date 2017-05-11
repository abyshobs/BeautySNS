using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IProfileDAO
    {
        Profile fetchByAccountID(int accountID);
        Profile fetchByJob(int jobID);
        void CreateProfile(Profile profile);
        void UpdateProfile(Profile profile);
        
    }
}
