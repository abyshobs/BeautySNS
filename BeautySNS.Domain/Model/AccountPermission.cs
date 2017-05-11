using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Model
{
    public class AccountPermission
    {
        public int accountPermissionID { get; set; }
        public int accountID { get; set; }
        public int permissionID { get; set; }
        public string email { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? lastUpdateDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
