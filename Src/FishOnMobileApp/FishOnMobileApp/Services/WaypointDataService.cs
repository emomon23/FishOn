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
        private IFishRepository _fishRepository;
        private IWeatherRepository _weatherRepository;

        public WayPointDataService()
        {
            _wayPointRepository = new WayPointRepository();
            _lakeRepository = new LakeRepository();
            _weatherService = new WeatherService();
            _fishRepository = new FishRepository();
            _weatherRepository = new WeatherRepository();
        }

        public WayPointDataService(IWayPointRepository wayPointRepository, ILakeRepository lakeRepository, IWeatherService weatherService, IFishRepository fishRepo, IWeatherRepository weatherRepo)
        {
            _wayPointRepository = wayPointRepository;
            _lakeRepository = lakeRepository;
            _weatherService = weatherService;
            _weatherRepository = weatherRepo;
            _fishRepository = fishRepo;
        }

        public async Task CreateNewFishOnAsync(double latitude, double longitude, Species speciesCaught)
        {
            var wayPoint = await _wayPointRepository.GetWayPointAsync(latitude, longitude);

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
       
        private async Task SaveAsync(WayPoint wayPoint)
        {
            if (wayPoint.WayPointId == 0)
            {
                await _wayPointRepository.SaveAsync(wayPoint);
            }

            var fishCaught = wayPoint.FishCaught.FirstOrDefault(f => f.FishOnId == 0);
            fishCaught.WayPointId = wayPoint.WayPointId;

            if (fishCaught.Lure != null && fishCaught.Lure.FishingLureId == 0)
            {
                var lure = fishCaught.Lure;
                await _fishRepository.SaveNewLureAsync(lure);
                fishCaught.FishingLureId = lure.FishingLureId;
            }

            await _fishRepository.SaveFishOnAsync(fishCaught);
            var weatherCondition = fishCaught.WeatherCondition;
            weatherCondition.WeatherConditionId = fishCaught.FishOnId;
            await _weatherRepository.SaveAsync(weatherCondition);
           
        }

        public async Task DeleteAsync(int wayPointId)
        {

        }
    }
}
