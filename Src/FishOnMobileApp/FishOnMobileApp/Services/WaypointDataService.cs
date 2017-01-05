using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;
using FishOn.Utils;

namespace FishOn.Services
{
    public interface IWayPointDataService
    {
        Task<List<WayPoint>> GetWayPointsAsync();
        Task CreateNewFishOnAsync(double latitude, double longitude, Species speciesCaught);
        Task SaveWayPointProvisioningAsync(WayPoint wayPoint);
        Task DeleteWayPointAsync(WayPoint wayPoint);
    }

    public class WayPointDataService : IWayPointDataService
    {
        private IWayPointRepository _wayPointRepository;
        private ILakeRepository _lakeRepository;
        private IWeatherService _weatherService;
        private IFishRepository _fishRepository;
        private IWeatherRepository _weatherRepository;
        private ISpeciesRepository _speciesRepository;

        public WayPointDataService()
        {
            _wayPointRepository = new WayPointRepository();
            _lakeRepository = new LakeRepository();
            _weatherService = new WeatherService();
            _fishRepository = new FishRepository();
            _weatherRepository = new WeatherRepository();
            _speciesRepository = new SpeciesRepository();
        }

        public WayPointDataService(IWayPointRepository wayPointRepository, ILakeRepository lakeRepository, IWeatherService weatherService, IFishRepository fishRepo, IWeatherRepository weatherRepo, ISpeciesRepository speciesRepository)
        {
            _wayPointRepository = wayPointRepository;
            _lakeRepository = lakeRepository;
            _weatherService = weatherService;
            _weatherRepository = weatherRepo;
            _fishRepository = fishRepo;
            _speciesRepository = speciesRepository;
        }

        public async Task SaveWayPointProvisioningAsync(WayPoint wayPoint)
        {
            await _wayPointRepository.SaveWayPointProvisioningAsync(wayPoint);
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
                    WayPointType = speciesCaught == null? WayPoint.WayPointTypeEnumeration.BoatLaunch : WayPoint.WayPointTypeEnumeration.FishOn
                };
            }

            if (speciesCaught != null)
            {
                var weatherCondition = await _weatherService.GetWeatherConditionsAsync(latitude, longitude);
                wayPoint.AddFishCaught(speciesCaught, weatherCondition);
            }

            await SaveAsync(wayPoint);
        }

        public async Task DeleteWayPointAsync(WayPoint wayPoint)
        {
            await _wayPointRepository.DeleteAsync(wayPoint);
        }

        public async Task<List<WayPoint>> GetWayPointsAsync()
        {
            var wayPoints = await _wayPointRepository.GetWayPointsAsync();
            var lakes = await _lakeRepository.GetLakesAsync();
            foreach (var wayPoint in wayPoints)
            {
                var lakeMatch = lakes.FirstOrDefault(l => l.LakeId == wayPoint.LakeId);
                wayPoint.Lake = (Lake)lakeMatch?.Clone<Lake>();

                var fishCaught = await _fishRepository.GetFishCaughtAtWayPointAsync(wayPoint.WayPointId);

                foreach (var fish in fishCaught)
                {
                    fish.Species = await _speciesRepository.GetSpeciesAsync(fish.SpeciesId);
                }

                wayPoint.FishCaught = fishCaught;
            }

            return wayPoints;
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
