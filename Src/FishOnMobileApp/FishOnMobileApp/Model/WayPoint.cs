﻿using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class WayPoint
    {
        public WayPoint()
        {
            FishCaught = new List<FishOn>();
            Species = new List<Species>();
        }

        [PrimaryKey, AutoIncrement]
        public int WayPointId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string WayPointType { get; set; }
        public int LakeId { get; set; }

        public virtual ICollection<FishOn> FishCaught { get; set; }
        public virtual ICollection<Species> Species { get; set; }

        public void AddFishCaught(Species speciesCaught, WeatherCondition weatherCondition)
        {
            var fishCaught = new FishOn()
            {
                DateCaught = DateTime.Now.AddMinutes(-5),
                Species = speciesCaught.Name,
                WaterTemp = SessionData.CurrentWaterTemp,
                WeatherCondition = weatherCondition
            };
            FishCaught.Add(fishCaught);

            if (!Species.Any(s => s.Name != speciesCaught.Name))
            {
                Species.Add(speciesCaught);
            }
        }
    }
}
