using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeautySNS.Admin.Models.StatusUpdates
{
    public class CreateViewModel
    {
        public int statusUpdateID { get; set; }
        public int accountID { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please provide a Name")]
        public string status { get; set; }

        public byte[] attachment { get; set; }
        public DateTime? createDate { get; set; }
        public bool userSession { get; set; }

    }
}