using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class Lake
    {
        [PrimaryKey, AutoIncrement]
        public int LakeId { get; set; }
        public string LakeName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Ignore]
        public virtual ICollection<WayPoint> WayPoints { get; set; }

       
    }
}
