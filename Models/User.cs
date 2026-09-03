using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Mngt_System.Models
{
    public class User
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string email_id { get; set; }
        public string user_password { get; set; }
        public string city { get; set; }
        public string phone_number { get; set; }

    }
}