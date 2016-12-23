using System.Collections.Generic;

namespace FishOn.Model
{
    public class Species
    {
        public int SpeciesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<WayPoint> WayPoints { get; set; }
    }
}
