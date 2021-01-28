using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidTrackerAndroid.Models
{
    public class LatLongGroup
    {

        public int LatLongGroupID { get; set; }
        public double NorthWestLat { get; set; }
        public double NorthWestLong { get; set; }
        public double SouthEastLat { get; set; }
        public double SouthEastLong { get; set; }
    }
}