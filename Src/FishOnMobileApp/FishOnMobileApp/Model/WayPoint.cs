using System.Collections.Generic;

namespace FishOn.Model
{
    public class WayPoint
    {
        public int WayPointId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string WayPointType { get; set; }
        public int LakeId { get; set; }

        public virtual ICollection<Species> Species { get; set; }
     
    }
}
