using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface IWayPointDataService
    {
        Task<List<WayPoint>> GetWayPointsAsync(int lakeId);
        Task CreateNewFishOnAsync(double latitude, double longitude, Species speciesCaught);
    }

    public class WayPointDataService : IWayPointDataService
    {
        private IWayPointRepository _wayPointRepository;
        private ILakeRepository _lakeRepository;
        private IWeatherService _weatherService;

        public WayPointDataService()
        {
            _wayPointRepository = new WayPointRepository();
            _lakeRepository = new LakeRepository();
            _weatherService = new WeatherService();
        }

        public WayPointDataService(IWayPointRepository wayPointRepository, ILakeRepository lakeRepository, IWeatherService weatherService)
        {
            _wayPointRepository = wayPointRepository;
            _lakeRepository = lakeRepository;
            _weatherService = weatherService;
        }

        public async Task CreateNewFishOnAsync(double latitude, double longitude, Species speciesCaught)
        {
            var wayPoint = await GetWayPoint(latitude, longitude);

            if (wayPoint == null)
            {
                int lakeId = SessionData.CurrentLakeId;

                wayPoint = new WayPoint()
                {
                    LakeId = lakeId,
                    Latitude = latitude,
                    Longitude = longitude,
                    WayPointType = speciesCaught == null? "BOAT LAUNCH" : "FISH"
                };
            }

            if (speciesCaught != null)
            {
                var weatherCondition = await _weatherService.GetWeatherConditionsAsync(latitude, longitude);
                wayPoint.AddFishCaught(speciesCaught, weatherCondition);
            }

            await SaveAsync(wayPoint);
        }

        public async Task<List<WayPoint>> GetWayPointsAsync(int lakeId)
        {
            return null;
        }
        
        private async Task<WayPoint> GetWayPoint(double latitude, double longitude)
        {
            var wayPoints = await _wayPointRepository.GetWayPointsAsync();
            return wayPoints?.SingleOrDefault(w => w.Longitude == longitude && w.Latitude == latitude);
        }

        private async Task SaveAsync(WayPoint wayPoint)
        {
            await _wayPointRepository.SaveAsync(wayPoint);
        }

        public async Task DeleteAsync(int wayPointId)
        {

        }
    }
}
