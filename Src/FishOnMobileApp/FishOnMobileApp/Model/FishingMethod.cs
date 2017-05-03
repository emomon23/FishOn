using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class FishingMethod
    {
        [PrimaryKey, AutoIncrement]
        public int FishingMethodId { get; set; }

        public string Description { get; set; }
    }
}
