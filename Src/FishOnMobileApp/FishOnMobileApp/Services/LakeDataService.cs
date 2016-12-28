using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface ILakeDataService
    {
        Task<List<Lake>> GetLakes();
        Task Save(Lake lake);
        Task Delete(int lakeId);
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

        public async Task<List<Lake>> GetLakes()
        {
            return null;
        }

        public async Task Save(Lake lake)
        {

        }

        public async Task Delete(int lakeId)
        {

        }
    }
}
