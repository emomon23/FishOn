using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface IWayPointRepository
    {
        Task<List<WayPoint>> GetWayPointsAsync(int lakeId);
        Task<List<WayPoint>> GetWayPointsAsync();
        Task SaveAsync(WayPoint wayPoint);
        Task DeleteAsync(int wayPointId);
    }

    public class WayPointRepository : BaseRepository, IWayPointRepository
    {
        private IFishOnHttpRepository _fishOnHttp;
        private List<WayPoint> _wayPoints = new List<WayPoint>();

        public WayPointRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp) { }

        public async Task<List<WayPoint>> GetWayPointsAsync(int lakeId)
        {
            return _wayPoints.Where(w => w.LakeId == lakeId).ToList();
        }

        public async Task<List<WayPoint>> GetWayPointsAsync()
        {
            return _wayPoints;
        }

        public async Task SaveAsync(WayPoint wayPoint)
        {
            if (wayPoint.WayPointId == 0)
            {
                wayPoint.WayPointId = _wayPoints.Count + 1;
                _wayPoints.Add(wayPoint);
            }
        }

        public async Task DeleteAsync(int wayPointId)
        {

        }
    }
}
