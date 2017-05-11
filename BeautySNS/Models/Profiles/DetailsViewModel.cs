using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Profiles
{
    public class DetailsViewModel
    {
        public DetailsViewModel() { }

        public DetailsViewModel(IEnumerable<Job> jobs) { }

        public DetailsViewModel(Profile profile)
        {
            Account = profile.Account;
            Job = profile.Job;
            jobID = profile.jobID;
            profileID = profile.profileID;
            accountID = profile.accountID;            
            avatar = profile.avatar;
            aboutMe = profile.aboutMe;
            education = profile.education;
            experience = profile.experience;
            website = profile.website;
            location = profile.location;
            createDate = profile.createDate;
            lastUpdateDate = profile.lastUpdateDate;
        }

        public int loggedInAccountID { get; set; }
        public int profileID { get; set; }
        public int accountID { get; set; }

        [DisplayName("Job")]
        public int jobID { get; set; }

        public string fullName { get; set; }

        [DisplayName("Profile Image")]
        public byte[] avatar { get; set; }

        public string avatarMIMEType { get; set; }

        [DisplayName("About Me")]
        public string aboutMe { get; set; }

        [DisplayName("Education")]
        public string education { get; set; }

        [DisplayName("Experience")]
        public string experience { get; set; }

        [DisplayName("Website")]
        public string website { get; set; }

        [DisplayName("Location")]
        public string location { get; set; }

        [DisplayName("First Name")]
        public string firstName { get; set; }

        [DisplayName("Last Name")]
        public string lastName { get; set; }

        [DisplayName("D.O.B")]
        public DateTime? birthDate { get; set; }

        [DisplayName("Date created")]
        public DateTime? createDate { get; set; }

        [DisplayName("Last updated")]
        public DateTime? lastUpdateDate { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }


        public Job Job { get; set; }
        public Account Account { get; set; }
        public Account LoggedInAccount { get; set; }
        public bool userSession { get; set; }
        public int userAccount { get; set; }

        public bool isFriend;
        public bool isNotFriend;
        public bool pendingRequest;
        public bool sentRequest;
        public bool sameAccount;
        public bool adminUser;
        public string permissionType { get; set; }
    }
}