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
        Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId);
        Task<List<Model.FishOn>> GetFishCaughtFromLure(int lureId);
        Task<List<Model.FishOn>> GetFishCaughtAsync();
        Task<List<FishingMethod>> GetFishingMethodsAsync();
        Task SaveFishingMethodAsync(FishingMethod method);
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

        public async Task<List<Model.FishOn>> GetFishCaughtAsync()
        {
            var db = await GetDB();
            var result = await db.GetFishCaught();

            return result;
        }

        public async Task<List<FishingMethod>> GetFishingMethodsAsync()
        {
            var db = await GetDB();
            return await db.GetFishingMethodsAsync();
        }

        public async Task SaveFishingMethodAsync(FishingMethod method)
        {
            var db = await GetDB();
            await db.SaveFishingMethodAsync(method);
        }

        public async Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId)
        {
            var db = await GetDB();
            var fishCaught = await db.GetFishCaughtAtWayPointAsync(wayPointId);

            return fishCaught;
        }

        public async Task<List<Model.FishOn>> GetFishCaughtFromLure(int lureId)
        {
            var db = await GetDB();
            var fishCaught = await db.GetFishCaughtOnLure(lureId);

            return fishCaught;
        }

        public async Task DeleteFishCaughtAsync(Model.FishOn fishCaught)
        {
            var db = await GetDB();
            await db.DeleteFishCaughtAsync(fishCaught);
        }
   
    }
}
