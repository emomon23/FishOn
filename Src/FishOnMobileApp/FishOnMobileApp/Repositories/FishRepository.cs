using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface IFishRepository
    {
        Task SaveFishOnAsync(Model.FishOn fishCaught);
        Task<bool> SaveLureAsync(FishingLure lure);
        Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId);
        Task<List<FishingLure>> GetAvailableLuresAsync();
        Task<FishingLure> GetFishingLureAsync(int fishingLureId);
        Task DeleteFishCaughtAsync(Model.FishOn fishCaught);
    }

    public class FishRepository: BaseRepository, IFishRepository
    {
        public FishRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp) { }

        public async Task SaveFishOnAsync(Model.FishOn fishCaught)
        {
            var db = await GetDB();
            await db.SaveFishOnAsync(fishCaught);
        }

        public async Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId)
        {
            var db = await GetDB();
            var fishCaught = await db.GetFishCaughtAtWayPointAsync(wayPointId);

            return fishCaught;
        }

        public async Task<bool> SaveLureAsync(FishingLure lure)
        {
            if (!lure.IsValid)
            {
                return false;
            }

            var db = await GetDB();
            await db.SaveLureAsync(lure);
            return true;
        }

        public async Task DeleteFishCaughtAsync(Model.FishOn fishCaught)
        {
            var db = await GetDB();
            await db.DeleteFishCaughtAsync(fishCaught);
        }

        public async Task<FishingLure> GetFishingLureAsync(int fishingLureId)
        {
            var db = await GetDB();
            return await db.GetLureAsync(fishingLureId);
        }

        public async Task<List<FishingLure>> GetAvailableLuresAsync()
        {
            var db = await GetDB();
            return await db.GetLuresAsync();
        }
    }
}
