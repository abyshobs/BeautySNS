using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class Permission
    {
        public int permissionID { get; set; }
        public string name { get; set; }

        public ICollection<AccountPermission> AccountPermissions { get; set; }
    }
}
