using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Code.Interfaces
{
    public interface IUserSession
    {
        bool LoggedIn { get; set; }
        Account CurrentUser { get; set; }
        string Email { get; set; }
        
    }
}
