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
        void RemoveFishFromCache(Model.FishOn fishCaught);
        void UpdateCache(Model.FishOn fishCaught);
    }

    public class WayPointDataService : IWayPointDataService
    {
        private List<WayPoint> _cachedWayPoints = null;
        private IWayPointRepository _wayPointRepository;
        private ILakeRepository _lakeRepository;
        private IWeatherService _weatherService;
        private IFishRepository _fishRepository;
        private IWeatherRepository _weatherRepository;
        private ISpeciesRepository _speciesRepository;
        private ISessionDataService _sessionDataService;

        public WayPointDataService()
        {
            _wayPointRepository = new WayPointRepository();
            _lakeRepository = new LakeRepository();
            _weatherService = new WeatherService();
            _fishRepository = new FishRepository();
            _weatherRepository = new WeatherRepository();
            _speciesRepository = new SpeciesRepository();
        }

        public WayPointDataService(IWayPointRepository wayPointRepository, ILakeRepository lakeRepository, IWeatherService weatherService, IFishRepository fishRepo, IWeatherRepository weatherRepo, ISpeciesRepository speciesRepository, ISessionDataService sessionDataService)
        {
            _wayPointRepository = wayPointRepository;
            _lakeRepository = lakeRepository;
            _weatherService = weatherService;
            _weatherRepository = weatherRepo;
            _fishRepository = fishRepo;
            _speciesRepository = speciesRepository;
            _sessionDataService = sessionDataService;
        }

        public async Task SaveWayPointProvisioningAsync(WayPoint wayPoint)
        {
            await SaveAsync(wayPoint);
        }

        public async Task CreateNewFishOnAsync(double latitude, double longitude, Species speciesCaught)
        {
            var wayPoint = await _wayPointRepository.GetWayPointAsync(latitude, longitude);

            if (wayPoint == null)
            {
                int lakeId = _sessionDataService.CurrentLake?.LakeId ?? 0;

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
                wayPoint.AddFishCaught(speciesCaught, weatherCondition, _sessionDataService.CurrentWaterTemp);
            }

            await SaveAsync(wayPoint);
        }

        public async Task DeleteWayPointAsync(WayPoint wayPoint)
        {
            await _wayPointRepository.DeleteAsync(wayPoint);
        }

        public async Task<List<WayPoint>> GetWayPointsAsync()
        {
            if (_cachedWayPoints == null)
            {
                var wayPoints = await _wayPointRepository.GetWayPointsAsync();
                var lakes = await _lakeRepository.GetLakesAsync();
                foreach (var wayPoint in wayPoints)
                {
                    var lakeMatch = lakes.FirstOrDefault(l => l.LakeId == wayPoint.LakeId);
                    wayPoint.Lake = (Lake) lakeMatch?.Clone<Lake>();

                    var fishCaught = await _fishRepository.GetFishCaughtAtWayPointAsync(wayPoint.WayPointId);

                    foreach (var fish in fishCaught)
                    {
                        fish.Species = await _speciesRepository.GetSpeciesAsync(fish.SpeciesId);
                        fish.WayPoint = (WayPoint) wayPoint.Clone<WayPoint>();

                        if (fish.FishingLureId.HasValue && fish.FishingLureId.Value != 0)
                        {
                            fish.Lure = await _fishRepository.GetFishingLureAsync(fish.FishingLureId.Value);
                        }
                        wayPoint.MergeSpecies(fish.Species);
                    }

                    wayPoint.FishCaught = fishCaught;
                }

                _cachedWayPoints = wayPoints;
            }

            return _cachedWayPoints;
        }

        public void RemoveFishFromCache(Model.FishOn fishCaught)
        {
            var wayPoint = _cachedWayPoints.SingleOrDefault(w => w.WayPointId == fishCaught.WayPointId);
            wayPoint.FishCaught.Remove(fishCaught);
        }

        public void UpdateCache(Model.FishOn fishCaught)
        {
            if (_cachedWayPoints != null)
            {
                _cachedWayPoints.MoveFishCaughtToDifferentWayPoint(fishCaught);
            }    
        }

        private async Task SaveAsync(WayPoint wayPoint)
        {
            if (wayPoint.WayPointId == 0)
            {
                await _wayPointRepository.SaveAsync(wayPoint);
            }

            var fishesCaught = wayPoint.FishCaught.Where(f => f.FishOnId == 0 || f.WayPointId == 0);
            foreach (var fishCaught in fishesCaught)
            {
                fishCaught.WayPointId = wayPoint.WayPointId;

                if (fishCaught.Lure != null && fishCaught.Lure.FishingLureId == 0)
                {
                    var lure = fishCaught.Lure;
                    if (lure.IsValid)
                    {
                        await _fishRepository.SaveLureAsync(lure);
                        fishCaught.FishingLureId = lure.FishingLureId;
                    }
                }

                await _fishRepository.SaveFishOnAsync(fishCaught);

                if (fishCaught.WeatherCondition != null)
                {
                    var weatherCondition = fishCaught.WeatherCondition;
                    weatherCondition.WeatherConditionId = fishCaught.FishOnId;
                    await _weatherRepository.SaveAsync(weatherCondition);
                }
                
            }

            if (_cachedWayPoints == null)
            {
                _cachedWayPoints = new List<WayPoint>();
            }

            if (!(_cachedWayPoints.Any(w => w.WayPointId == wayPoint.WayPointId)))
            {
                _cachedWayPoints.Add(wayPoint);
            }
           
        }

        public async Task DeleteAsync(int wayPointId)
        {

        }
    }
}
