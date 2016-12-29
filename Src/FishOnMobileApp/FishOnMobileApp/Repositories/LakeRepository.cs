using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface ILakeRepository
    {
        Task<List<Lake>> GetLakesAsync();
        Task SaveAsync(Lake lake);
        Task DeleteAsync(int lakeId);
    }

    public class LakeRepository : BaseRepository, ILakeRepository
    {
        private readonly IFishOnHttpRepository _fishOnHttp;

        public LakeRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp) { }


        public async Task<List<Lake>> GetLakesAsync()
        {
            return null;
        }

        public async Task SaveAsync(Lake lake)
        {
            
        }

        public async Task DeleteAsync(int lakeId)
        {
            
        }
    }
}
