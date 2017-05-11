using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class Job
    {
        public int jobID { get; set; }
        public string name { get; set; }

        public ICollection<Profile> Profiles { get; set; }
    }
}
