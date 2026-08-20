using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Mngt_System.Models
{
    public class Movie
    {
        public int movie_id { get; set; }
        public string movie_name { get; set; }
        public string relese_date { get; set; }
        public int cat_id { get; set; }
        public int rate { get; set; }
    }
}