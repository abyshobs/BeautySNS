using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO;
using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BeautySNS.Domain.Code
{
    public class Email : IEmail
    {

        const string TO_EMAIL_ADDRESS = "beautysnsuk@gmail.com";         
        const string FROM_EMAIL_ADDRESS = "beautysnsuk@gmail.com";

        private IConfiguration configuration;
        private FriendInvitationDAO friendInvitationDAO;
        private readonly BSNSContext _db;

        public Email(IConfiguration configuration, FriendInvitationDAO friendInvitationDAO, BSNSContext db)
        {
            this.configuration = configuration;
            this.friendInvitationDAO = friendInvitationDAO;
            _db = db;
        }
        
        public void SendEmail(string From, string Subject, string Message) 
        {                     
          MailMessage mm = new MailMessage(From,TO_EMAIL_ADDRESS);
          mm.Subject = Subject;             
          mm.Body = Message;             
          Send(mm);    
        }
            
        public void SendEmail(string To, string CC, string BCC, string Subject, string Message) 
        {
            MailMessage mm = new MailMessage(FROM_EMAIL_ADDRESS,To);    
            //mm.To.Add(To);
            //mm.CC.Add(CC);             
            //mm.Bcc.Add(BCC);             
            if (!string.IsNullOrEmpty(CC))
                mm.CC.Add(CC);

            if (!string.IsNullOrEmpty(BCC))
                mm.Bcc.Add(BCC);

            mm.Subject = Subject;             
            mm.Body = Message;            
            mm.IsBodyHtml = true;             
            Send(mm);        
        }        
        
        public void SendEmail(string[] To, string[] CC, string[] BCC, string Subject, string Message)
            { 
              MailMessage mm = new MailMessage();             
              
            foreach (string to in To)             
              { 
                  mm.To.Add(to);              
              }             
             
            foreach (string cc in CC) 
             {                 
                 mm.CC.Add(cc);            
             }            
            
            foreach (string bcc in BCC)            
            {                 
                mm.Bcc.Add(bcc);           
            }            
            
            mm.From = new MailAddress(FROM_EMAIL_ADDRESS);            
            mm.Subject = Subject;            
            mm.Body = Message;            
            mm.IsBodyHtml = true;            
            Send(mm);   
        }
        
        public void SendIndividualEmailsPerRecipient(string[] To, string Subject, string Message) 
        {             
            foreach (string to in To)             
            {                
                MailMessage mm = new MailMessage(FROM_EMAIL_ADDRESS,to); 
                mm.Subject = Subject;                
                mm.Body = Message;                 
                mm.IsBodyHtml = true;                 
                Send(mm);             
            }         
        }         
        
        //private void Send(MailMessage Message)        
        //{            
        //    SmtpClient smtp = new SmtpClient();             
        //    smtp.Send(Message);        
        //}


        private void Send(MailMessage Message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("signatureVirginHair@gmail.com", "Faosat63"),
                EnableSsl = true
            };
            client.Send(Message);
        }



        public void SendEmailAddressVerificationEmail(string email, string To)
        {
            string msg = "Please click on the link below or paste it into a browser to verify your email account.<BR><BR>" +
                            "<a href=\"" + configuration.RootURL + "Account/VerifyEmail?a=" +
                            HttpUtility.UrlEncode(email.Encrypt("verify")) + "\">" +
                            configuration.RootURL + "Account/VerifyEmail?a=" +
                            HttpUtility.UrlEncode(email.Encrypt("verify")) + "</a>";
                            
            SendEmail(To, "", "", "Account created! Email verification required.", msg);
            //Cryptography.Encrypt(Username, "verify")
        }

        public void PasswordRecoveryEmail(string email, string To)
        {
            string msg = "Please click on the link below or paste it into a browser to change your password.<BR><BR>" +
                            "<a href=\"" + configuration.RootURL + "Account/ChangePassword?a=" +
                            HttpUtility.UrlEncode(email.Encrypt("verify")) + "\">" +
                            configuration.RootURL + "Account/ChangePassword?a=" +
                            HttpUtility.UrlEncode(email.Encrypt("verify")) + "</a>";

            SendEmail(To, "", "", "You requested to change your BeautySNS password!.", msg);
        }

        public void SendChangedEmailAddressVerificationEmail(string email, string To)
        {
            string msg = "Please click on the link below or paste it into a browser to verify your email account.<BR><BR>" +
                            "<a href=\"" + configuration.RootURL + "Account/VerifyChangedEmail?a=" +
                            HttpUtility.UrlEncode(email.Encrypt("verify")) + "\">" +
                            configuration.RootURL + "Account/VerifyChangedEmail?a=" +
                            HttpUtility.UrlEncode(email.Encrypt("verify")) + "</a>";

            SendEmail(To, "", "", "You asked to change your email! Email verification required.", msg);
            //Cryptography.Encrypt(Username, "verify")
        }

        public string SendInvitations(Account sender, string ToEmailArray, string Message)
        {
            string resultMessage = Message;
            foreach (string s in ToEmailArray.Split(new char[] { ',', ';' }))
            {
                FriendInvitation friendInvitation = new FriendInvitation();
                friendInvitation.accountID = sender.accountID;
                friendInvitation.email = s;
                friendInvitation.GUID = Guid.NewGuid();
                friendInvitation.becameAccountID = 0;
                friendInvitationDAO.CreateInvitation(friendInvitation);
                //_db.SaveChanges();

                SendFriendInvitation(s, sender.firstName, sender.lastName, friendInvitation.GUID.ToString(), Message);
                
                resultMessage += "• " + s + "<BR>";
            }
            return resultMessage;
        }

        public void SendFriendInvitation(string toEmail, string fromFirstName, string fromLastName, string GUID, string Message)
        {
            Message = fromFirstName + " " + fromLastName +
            " has invited you to join us at BeautySNS " + "!<HR><a href=\"" + configuration.RootURL +
            "Friend/ConfirmFriendRequest?InvitationKey=" + GUID + "\">" + configuration.RootURL +
            "Friend/ConfirmFriendRequest?InvitationKey=" + GUID + "</a><HR>" + Message;

            SendEmail(toEmail, "", "", fromFirstName + " " + fromLastName +
                " has invited you to join us at BeautySNS" + "!", Message);
        }


    }
}
