using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

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
