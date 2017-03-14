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
            imageIcon = "fish.png";
        }

        [PrimaryKey, AutoIncrement]
        public int SpeciesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailableOnCatchList { get; set; }
        public int DisplayOrder { get; set; }
        public bool DisplaySpeciesOnLakeMap { get; set; }
        public bool IsDeleted { get; set; }
        public string imageIcon { get; set; }
    }
}
