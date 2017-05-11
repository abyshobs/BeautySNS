using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO
{
    public class JobDAO : IJobDAO
    {
        //creates an instance of the database
            private readonly BSNSContext _db;

            public JobDAO(BSNSContext db)
            {
                _db = db;
            }

            //creates a job and adds it to the database
            public void Create(Job job)
            {
                _db.Jobs.Add(job);
                _db.SaveChanges();
            }

            //updates an existing job
            public void Update(Job job)
            {
                Job originalJob = _db.Jobs.Find(job.jobID);
                originalJob.name = job.name;

                _db.SaveChanges();
            }

            //fetches all the jobs
            public IEnumerable<Job> FetchAll()
            {
                return _db.Jobs.ToList();
            }

            //fetches a job by its id
            public Job FetchById(int id)
            {
                return _db.Jobs.FirstOrDefault(j => j.jobID == id);
            }

            //fetches a job by its name
            public Job FetchByName(string name)
            {
                return _db.Jobs.FirstOrDefault(j => j.name == name);
            }

           //deletes a job
            public void Delete(int id)
            {
                Job job = _db.Jobs.Find(id);
                _db.Jobs.Remove(job);
                _db.SaveChanges();
            }

    }
}
