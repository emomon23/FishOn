﻿using System.Collections.Generic;

namespace FishOn.Model
{
    public class Lake
    {
        public int LakeId { get; set; }
        public string LakeName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual ICollection<Species> Species { get; set; }
        public virtual ICollection<WayPoint> WayPoints { get; set; }
    }
}
