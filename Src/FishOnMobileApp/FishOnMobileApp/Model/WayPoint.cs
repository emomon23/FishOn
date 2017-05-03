using System;
using System.Collections.Generic;
using System.Linq;
using FishOn.Utils;
using SQLite;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class WayPoint
    {
        public enum WayPointTypeEnumeration
        {
            BoatLaunch,
            FishOn
        }

        public WayPoint()
        {
            FishCaught = new List<FishOn>();
            Species = new List<Species>();
            Name = $"WP-{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 7)}";
            DateFirstCreated = DateTime.Now;
        }

        [PrimaryKey, AutoIncrement]
        public int WayPointId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public DateTime DateFirstCreated { get; set; }
        public string Notes { get; set; }
        public WayPointTypeEnumeration WayPointType { get; set; }
        public int LakeId { get; set; }

        [Ignore]
        public virtual ICollection<FishOn> FishCaught { get; set; }

        [Ignore]
        public virtual ICollection<Species> Species { get; set; }

        [Ignore]
        public virtual Lake Lake { get; set; }

        public void AddFishCaught(Species speciesCaught, int currentWaterTemp)
        {
            var fishCaught = new FishOn()
            {
                DateCaught = DateTime.Now.AddMinutes(-5),
                SpeciesId = speciesCaught.SpeciesId,
                WaterTemp = currentWaterTemp.ToString(),
               Species = speciesCaught
            };
            FishCaught.Add(fishCaught);

            if (!Species.Any(s => s.Name != speciesCaught.Name))
            {
                Species.Add(speciesCaught);
            }
        }

        public void MergeSpecies(Species species) 
        {
            if (!Species.Any(s => s.SpeciesId == species.SpeciesId))
            {
                Species.Add((Species)species.Clone<Species>());
            }
        }
    }
}
