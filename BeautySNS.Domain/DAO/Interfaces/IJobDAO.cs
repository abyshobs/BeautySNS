using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO.Interfaces
{
    public interface IJobDAO
    {
         void Create(Job job);
         void Update(Job job);
         IEnumerable<Job> FetchAll();
         Job FetchById(int id);
         Job FetchByName(string name);
         void Delete(int id);
    }
}
