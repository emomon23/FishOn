using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface IAppSettingRepo
    {
        Task<List<AppSetting>> GetAppSettingsAsync();
        Task SaveSettingAsync(AppSetting setting);
    }

    public class AppSettingRepo : BaseRepository, IAppSettingRepo
    {
        public AppSettingRepo() : base(null) { }

        public async Task<List<AppSetting>>  GetAppSettingsAsync()
        {
            var db = await GetDB();
            return await db.GetAppSettingsAsync();
        }

        public async Task SaveSettingAsync(AppSetting setting)
        {
            var db = await GetDB();
            await db.SaveAppSettingAsync(setting);
        }
    }
}
