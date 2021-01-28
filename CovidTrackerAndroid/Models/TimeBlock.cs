using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidTrackerAndroid.Models
{
    public class TimeBlock
    {
        public int TimeBlockID { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}