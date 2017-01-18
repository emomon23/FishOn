using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface ILakeRepository
    {
        Task<List<Lake>> GetLakesAsync();
        Task<Lake> GetLakeAsync(string lakeName);
        Task<Lake> GetLakeAsync(int lakeId);
        Task SaveAsync(List<Lake> lake);
        Task DeleteAsync(Lake lake);
    }

    public class LakeRepository : BaseRepository, ILakeRepository
    {
        public LakeRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp) { }
        
        public async Task<List<Lake>> GetLakesAsync()
        {
            var db = await GetDB();
            var result = await db.GetLakesAsync();
            return result;
        }

        public async Task<Lake> GetLakeAsync(string lakeName)
        {
            var db = await GetDB();
            var result = await db.GetLakeAsync(lakeName);
            return result;
        }

        public async Task<Lake> GetLakeAsync(int lakeId)
        {
            var db = await GetDB();
            var result = await db.GetLakeAsync(lakeId);
            return result;
        }


        public async Task SaveAsync(List<Lake> lakes)
        {
            var db = await GetDB();
            await db.SaveNewLakesAsync(lakes);
        }

        public async Task DeleteAsync(Lake lake)
        {
            var db = await GetDB();
            await db.DeleteLakeAsync(lake);
        }
    }
}
