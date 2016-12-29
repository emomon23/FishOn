using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface ILakeDataService
    {
        Task<List<Lake>> GetLakesAsync();
        Task SaveAsync(Lake lake);
        Task DeleteAsync(int lakeId);
        Task<int> FindClosestLakeIdAsync(double latitude, double longitude);
    }

    public class LakeDataService : ILakeDataService
    {
        private ILakeRepository _lakeRepo;

        public LakeDataService()
        {
            _lakeRepo = new LakeRepository();
        }

        public LakeDataService(ILakeRepository lakeRepo)
        {
            _lakeRepo = lakeRepo;
        }

        public async Task<List<Lake>> GetLakesAsync()
        {
            return null;
        }

        public async Task<int> FindClosestLakeIdAsync(double latitude, double longitude)
        {
            return -1;
        }

        public async Task SaveAsync(Lake lake)
        {

        }

        public async Task DeleteAsync(int lakeId)
        {

        }
    }
}
