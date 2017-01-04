using System.Collections.Generic;
using SQLite;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class Species
    {
        public Species()
        {
            IsVisible = true;
        }

        [PrimaryKey, AutoIncrement]
        public int SpeciesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public int DisplayOrder { get; set; }
    }
}
