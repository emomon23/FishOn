using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface IWayPointDataService
    {
        Task<List<WayPoint>> GetWayPoints(int lakeId);
        Task Save(WayPoint wayPoint);
        Task Delete(int wayPointId);
    }

    public class WayPointDataService : IWayPointDataService
    {
        private IWayPointRepository _wayPointRepository;

        public WayPointDataService()
        {
            _wayPointRepository = new WayPointRepository();
        }

        public WayPointDataService(IWayPointRepository wayPointRepository)
        {
            _wayPointRepository = wayPointRepository;
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
