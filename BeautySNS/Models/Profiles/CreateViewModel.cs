using BeautySNS.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.Profiles
{
    public class CreateViewModel
    {
        public CreateViewModel(){}
        public CreateViewModel(IEnumerable<Job> jobs)
        {
          Jobs = jobs;
        }

        public IEnumerable<Job> Jobs {get; set;}
        public int profileID { get; set; }
        public int accountID { get; set; }

        [Required(ErrorMessage = "Please provide a Job")]
        [DisplayName("Job")]
        public int jobID { get; set; }


        public byte[] avatar { get; set; }
        public string avatarMIMEType { get; set; }

        [Required(ErrorMessage = "Please tell us a little about yourself")]
        [DisplayName("About Me")]
        public string aboutMe { get; set; }

        [Required(ErrorMessage = "If you have no previous education please write 'None'")]
        [DisplayName("Education")]
        public string education { get; set; }

        [Required(ErrorMessage = "If you have no experience please write 'None'")]
        [DisplayName("Experience")]
        public string experience { get; set; }


        [DisplayName("Website")]
        public string website { get; set; }

        [Required(ErrorMessage = "Please provide a Location")]
        [DisplayName("Location")]
        public string location { get; set; }

        public bool userSession { get; set; }
        public bool adminUser { get; set; }
        public int loggedInAccountID { get; set; }
    }
}