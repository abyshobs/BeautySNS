using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Profiles
{
    public class EditViewModel
    {
        public EditViewModel() { }

        public EditViewModel(Profile profile, IEnumerable<Job> jobs)
        {
            Account = profile.Account;
            Job = profile.Job;
            Jobs = jobs;
            profileID = profile.profileID;
            accountID = profile.accountID;
            firstName = profile.Account.firstName;
            lastName = profile.Account.lastName;
            birthDate = profile.Account.birthDate;
            jobID = profile.jobID;
            avatar = profile.avatar;
            education = profile.education;
            experience = profile.experience;
            website = profile.website;
            location = profile.location;
            aboutMe = profile.aboutMe;
          
        }

        public int profileID { get; set; }
        public int accountID { get; set; }

        [DisplayName("Job")]
        public int jobID { get; set; }

        [DisplayName("Profile Image")]
        public byte[] avatar { get; set; }

        public string avatarMIMEType { get; set; }

        [DisplayName("About Me")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please tell us a little about yourself")]
        public string aboutMe { get; set; }

        [DisplayName("Education")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "If you have no previous education please write 'None'")]
        public string education { get; set; }

        [DisplayName("Experience")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "If you have no previous experience please write 'None'")]
        public string experience { get; set; }

        [DisplayName("Website")]
        public string website { get; set; }

        [DisplayName("Location")]
        [Required(ErrorMessage = "Please provide your Location")]
        public string location { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("First Name")]
        public string firstName { get; set; }

        [DisplayName("Last Name")]
        public string lastName { get; set; }

        [DisplayName("D.O.B")]
        public DateTime? birthDate { get; set; }

        public IEnumerable<Job> Jobs { get; set; }

        public virtual Account Account { get; set; }
        public virtual Job Job { get; set; }
        public bool userSession { get; set; }
        public Account loggedInAccount { get; set; }
        public int loggedInAccountID { get; set; }
        public bool adminUser { get; set; }
    }
}