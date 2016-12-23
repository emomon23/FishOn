using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Services
{
    public interface IWayPointDataService
    {
        Task<List<WayPoint>> GetWayPoints(int lakeId);
        Task Save(WayPoint wayPoint);
        Task Delete(int wayPointId);
    }

    public class WayPointRepository : IWayPointDataService
    {
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
