using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Services.Interfaces
{
    public interface IAccountService
    {
        bool EmailInUse(string email);
        void Logout();
    }
}
