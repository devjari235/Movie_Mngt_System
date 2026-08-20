using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Mngt_System.Models
{
    public class Booking
    {
        public int booking_id { get; set; }
        public int user_id { get; set; }
        public int cat_id { get; set; }
        public int movie_id { get; set; }
        public int no_of_ticket { get; set; }
        public int amount { get; set; }
    }
}