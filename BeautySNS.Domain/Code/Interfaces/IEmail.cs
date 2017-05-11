using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.Code.Interfaces
{
    public interface IEmail
    {
        void SendEmail(string From, string Subject, string Message);
        void SendEmail(string To, string CC, string BCC, string Subject, string Message);
        void SendEmail(string[] To, string[] CC, string[] BCC, string Subject, string Message);
        void SendIndividualEmailsPerRecipient(string[] To, string Subject, string Message);
        void SendEmailAddressVerificationEmail(string firstName, string To);
        void SendChangedEmailAddressVerificationEmail(string firstName, string To);
        void PasswordRecoveryEmail(string email, string To);
        string SendInvitations(Account sender, string ToEmailArray, string Message);
        void SendFriendInvitation(string toEmail, string fromFirstName, string fromLastName, string GUID, string Message);

    }
}
