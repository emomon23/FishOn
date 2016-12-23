using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface IWayPointRepository
    {
        Task<List<WayPoint>> GetWayPoints(int lakeId);
        Task Save(WayPoint wayPoint);
        Task Delete(int wayPointId);
    }

    public class WayPointRepository : IWayPointRepository
    {
        private IFishOnHttpRepository _fishOnHttp;

        public WayPointRepository(IFishOnHttpRepository fishOnHttp)
        {
            _fishOnHttp = fishOnHttp;
        }

        public async Task<List<WayPoint>> GetWayPoints(int lakeId)
        {
            return null;
        }

        public async Task Save(WayPoint lake)
        {

        }

        public async Task Delete(int wayPointId)
        {

        }
    }
}
