using System.Collections.Generic;
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

        public WayPointRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp) { }

        public async Task<List<WayPoint>> GetWayPointsAsync(int lakeId)
        {
            return null;
        }

        public async Task<List<WayPoint>> GetWayPointsAsync()
        {
            return null;
        }

        public async Task SaveAsync(WayPoint lake)
        {

        }

        public async Task DeleteAsync(int wayPointId)
        {

        }
    }
}
