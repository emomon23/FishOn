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
        Task<WayPoint> GetWayPointAsync(double latitude, double longitude);
        Task SaveAsync(WayPoint wayPoint);
        Task SaveWayPointProvisioningAsync(WayPoint wayPoint);
        Task DeleteAsync(WayPoint wayPoint);
    }

    public class WayPointRepository : BaseRepository, IWayPointRepository
    {
        public WayPointRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp) { }

        public async Task<List<WayPoint>> GetWayPointsAsync(int lakeId)
        {
            var db = await GetDB();
            return await db.GetWayPointsAsync(lakeId);
        }

        public async Task<List<WayPoint>> GetWayPointsAsync()
        {
            var db = await GetDB();
            return await db.GetWayPointsAsync();
        }

        public async Task<WayPoint> GetWayPointAsync(double latitude, double longitude)
        {
            var db = await GetDB();
            return await db.GetWayPointAsync(latitude, longitude);
        }
        

        public async Task SaveAsync(WayPoint wayPoint)
        {
            var db = await GetDB();
            if (wayPoint.WayPointId == 0)
            {
                await db.SaveWayPointAsync(wayPoint);
            }
        }

        public async Task SaveWayPointProvisioningAsync(WayPoint wayPoint)
        {
            var db = await GetDB();
            await db.SaveWayPointAsync(wayPoint);
        }

        public async Task DeleteAsync(WayPoint wayPoint)
        {
            if (wayPoint?.WayPointId > 0)
            {
                var db = await GetDB();
                await db.DeleteWayPointAsync(wayPoint);
            }
        }
    }
}
