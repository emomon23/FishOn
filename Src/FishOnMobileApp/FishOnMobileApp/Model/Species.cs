using System.Collections.Generic;
using SQLite;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class Species
    {
        public Species()
        {
            IsAvailableOnCatchList = true;
        }

        [PrimaryKey, AutoIncrement]
        public int SpeciesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailableOnCatchList { get; set; }
        public int DisplayOrder { get; set; }
        public bool DisplaySpeciesOnLakeMap { get; set; }
    }
}
