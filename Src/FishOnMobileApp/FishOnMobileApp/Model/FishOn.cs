using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Utils;
using SQLite;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class FishOn
    {
        private string _dateTimeCaught = DateTime.Now.AddMinutes(-5).ToString("g");
        private int _wayPointId = 0;
        private int _speciesId = 0;
        private int _lakeId = 0;
        private int _originalWayPointId = 0;
        private int _originalSpeciesId = 0;
        private WayPoint _wayPoint;
        private Species _species;

        public FishOn()
        {
            Species = new Species();
        }

        [PrimaryKey, AutoIncrement]
        public int FishOnId { get; set; }

        [Ignore]
        public DateTime DateCaught
        {
            get { return _dateTimeCaught.ToDate(); }
            set { _dateTimeCaught = value.ToString("g"); }
        }

        public string DateTimeCaughtString
        {
            get { return _dateTimeCaught; }
            set
            {
                if (value.IsDate())
                {
                    _dateTimeCaught = value;
                }
            }
        }

        [Indexed]
        public int FishingMethodId { get; set; }

        public string WeatherConditions { get; set; }

        public string MoonPercentage { get; set; }

        public string WaterTemp { get; set; }

     
        [Indexed]
        public int? FishingLureId { get; set; }

        [Indexed]
        public int WayPointId
        {
            get { return _wayPointId; }
            set
            {
                _wayPointId = value;
                if (_originalWayPointId == 0)
                {
                    _originalWayPointId = value;
                }
            }
        }
     
        public bool WayPointIsDirty
        {
            get { return _originalWayPointId != 0 && _originalWayPointId != _wayPointId; }
        }

        public bool SpeciesIsDirty
        {
            get { return _originalSpeciesId != 0 && _originalSpeciesId != _speciesId; }
        }

        public int OriginalWayPointId { get { return _originalWayPointId;} }

        public void ResetDirtyFlags()
        {
            _originalWayPointId = _wayPointId;
            _originalSpeciesId = _speciesId;
        }

        public string Image1File { get; set; }

        public string Image2File { get; set; }

        [Indexed]
        public int SpeciesId {
            get
            {
                return _speciesId;   
            }
            set
            {
                _speciesId = value;
                if (_originalSpeciesId == 0)
                {
                    _originalSpeciesId = value;
                }
            }
        }
        
        [Ignore]
        public virtual WayPoint WayPoint
        {
            get
            {
                return _wayPoint;
            }
            set
            {
                _wayPoint = value;
                WayPointId = value.WayPointId;
            }
        }

        [Ignore]
        public virtual Species Species
        {
            get
            {
                return _species;
            }
            set
            {
                _species = value;
                SpeciesId = value.SpeciesId;
            }
        }
    }
}
