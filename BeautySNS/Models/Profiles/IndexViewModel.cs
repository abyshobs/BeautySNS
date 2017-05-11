using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Profiles
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }

        public IndexViewModel(IEnumerable<BeautySNS.Domain.Model.Profile> profiles)
        {
            Profiles = profiles;
        }

        public IEnumerable<BeautySNS.Domain.Model.Profile> Profiles { get; set; }
    }
}