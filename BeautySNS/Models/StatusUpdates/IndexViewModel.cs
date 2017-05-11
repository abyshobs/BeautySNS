using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.StatusUpdates
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }

        public IndexViewModel(IEnumerable<BeautySNS.Domain.Model.StatusUpdate> statusUpdates)
        {
            StatusUpdates = statusUpdates;
        }

        public IEnumerable<BeautySNS.Domain.Model.StatusUpdate> StatusUpdates { get; set; }
    }
}