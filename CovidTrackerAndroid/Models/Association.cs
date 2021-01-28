using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CovidTrackerAndroid.Models
{
    public class Association
    {
        public int AssociationID { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual int TimeBlockID { get; set; }
        public virtual int LatLongGroupID { get; set; }
    }
}