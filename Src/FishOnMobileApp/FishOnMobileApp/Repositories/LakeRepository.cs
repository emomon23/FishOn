using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface ILakeRepository
    {
        Task<List<Lake>> GetLakes();
        Task Save(Lake lake);
        Task Delete(int lakeId);
    }

    public class LakeRepository : ILakeRepository
    {
        private readonly IFishOnHttpRepository _fishOnHttp;

        public LakeRepository(IFishOnHttpRepository fishOnHttp)
        {
            _fishOnHttp = fishOnHttp;
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
