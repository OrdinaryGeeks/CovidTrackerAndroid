using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidTrackerAndroid.Models
{
    public class User
    {


        public int UserID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public long PhoneNumber { get; set; }

        public ICollection<Association> Associations;
    }
}