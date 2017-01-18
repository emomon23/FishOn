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
        private int _originalWayPointId = 0;
        private int _originalSpeciesId = 0;
        private WayPoint _wayPoint;
        private FishingLure _lure;
        private Species _species;

        public FishOn()
        {
            WeatherCondition = new WeatherCondition();
            Species = new Species();
            Lure = new FishingLure();
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

        public string Note { get; set; }
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
     
        public string Method { get; set; }

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
        public WeatherCondition WeatherCondition { get; set; }

        [Ignore]
        public virtual FishingLure Lure
        {
            get
            {
                return _lure;
            }
            set
            {
                _lure = value;
                FishingLureId = value.FishingLureId;
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
