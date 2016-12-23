using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface IFishOnHttpRepository
    {
        Task HttpPost(Lake lake);
        Task HttpPost(WayPoint lake);
        Task HttpPost(Species lake);
        Task<List<Species>> GetSpecies();
        Task<List<Species>> GetSpecies(int lakeId);
        Task<List<WayPoint>> GetWayPoints(int lakeId);
    }

    public class FishOnHttpRepository : IFishOnHttpRepository
    {
        private const string Base_Service_Url = "";

        public async Task HttpPost(Lake lake)
        {

        }

        public async Task HttpPost(WayPoint lake)
        {

        }

        public async Task HttpPost(Species lake)
        {

        }
        
        public async Task<List<Species>> GetSpecies()
        {
            return null;
        }

        public async Task<List<Species>> GetSpecies(int lakeId)
        {
            return null;
        }

        public async Task<List<WayPoint>> GetWayPoints(int lakeId)
        {
            return null;
        }

    }
}
