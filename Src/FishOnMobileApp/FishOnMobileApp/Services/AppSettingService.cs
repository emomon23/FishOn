using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;
using FishOn.Utils;

namespace FishOn.Services
{
    public interface IAppSettingService
    {
        Task Initialize();
        bool MapFilterHasBeenUsed { get; set; }
    }

    public class AppSettingService : IAppSettingService
    {
        private IAppSettingRepo _repo;
        private List<AppSetting> _appSettings;

        public AppSettingService()
        {
            _repo = new AppSettingRepo();
        }

        public AppSettingService(IAppSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task Initialize()
        {
            _appSettings = await _repo.GetAppSettingsAsync();
        }

        private string MapFilterHasBeenUsedKey = "MapFilterHasBeenUsed";
        public bool MapFilterHasBeenUsed
        {
            get
            {
                var stringValue = _appSettings.GetStringValue(MapFilterHasBeenUsedKey);
                return stringValue.ToBool();
            }
            set
            {
                var setting = _appSettings.SingleOrDefault(s => s.SettingName == MapFilterHasBeenUsedKey);
                if (setting != null)
                {
                    setting.Value = value.ToString();
                    Task.Run(() => { _repo.SaveSettingAsync(setting); });
                }
            }
        }
    }
}
