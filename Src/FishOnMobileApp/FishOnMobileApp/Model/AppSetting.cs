using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class AppSetting
    {
        [PrimaryKey, AutoIncrement]
        public int AppSettingId { get; set; }

        public string SettingName { get; set; }

        public string Value { get; set; }
    }
}
