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
        Task SaveNewLureAsync(FishingLure lure);
        Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId);
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

        public async Task SaveNewLureAsync(FishingLure lure)
        {
            var db = await GetDB();
            await db.SaveNewLureAsync(lure);
        }
    }
}
