using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Model.ViewModel;
using FishOn.Repositories;
using FishOn.Utils;

namespace FishOn.Services
{
    public interface IFishOnService
    {
        Task<List<Species>> GetSpeciesListAsync();
        Task<List<WayPoint>> GetWayPointsAsync();
        Task<List<Lake>> GetLakesAsync();
        Task<NewFishOnViewModel> GetMostRecentFishCaughtAsync(int preferredButNotRequiredSpeciesId, int maxMinsOld);
        Task<List<AppSetting>> GetAppSettingsAsync();
        Task<List<FishingMethod>> GetFishingMethodsAsync();

        Task<List<Lake>> CreateNewLakesAsync(params string[] lakeNames);

        Task SaveLakeAsync(Lake lake);
        Task SaveFishCaughtAsync(NewFishOnViewModel fishCaught);
        Task SaveWayPointProvisioningAsync(WayPoint wp);
        Task SaveSpeciesAsync(Species species);

        Task DeleteLakeAsync(Lake lake);
        Task DeleteWayPointAsync(WayPoint wp);
        Task DeleteSpeciesAsync(Species species);

        bool HasMapFilterEverBeenUsed { get; set; }
    }

    public class FishOnService : IFishOnService
    {
        private List<Lake> _cachedLakes = null;
        private List<WayPoint> _cachedWayPoints = null;
        private List<Species> _cachedSpecies = null;
        private List<FishingMethod> _cachedMethods = null;
       
        private IRepoFactory _repoFactory;

        public FishOnService(IRepoFactory repoFactory = null)
        {
            _repoFactory = repoFactory ?? new RepoFactory();
        }

        public async Task<List<FishingMethod>> GetFishingMethodsAsync()
        {
            if (_cachedMethods == null)
            {
                _cachedMethods = await _repoFactory.FishRepo.GetFishingMethodsAsync();
            }

            return _cachedMethods;
        }

        public async Task<List<Species>> GetSpeciesListAsync()
        {
            if (_cachedSpecies == null)
            {
                _cachedSpecies = await _repoFactory.SpeciesRepository.GetSpeciesAsync();
            }

            return _cachedSpecies;
        }

        public async Task<List<WayPoint>> GetWayPointsAsync()
        {
            if (_cachedWayPoints == null)
            {
                _cachedWayPoints = await _repoFactory.WayPointRepository.GetWayPointsAsync();
            }

            return _cachedWayPoints;
        }

        public async Task<List<Lake>> GetLakesAsync()
        {
            if (_cachedLakes == null)
            {
                _cachedLakes = await _repoFactory.LakeRepo.GetLakesAsync();
            }

            return _cachedLakes;
        }

        public async Task<NewFishOnViewModel> GetMostRecentFishCaughtAsync(int preferredButNotRequiredSpeciesId, int maxMinsOld)
        {
            var fishCaught = (await _repoFactory.FishRepo.GetFishCaughtAsync()).OrderByDescending(f => f.DateCaught);

            var lastCatch = fishCaught.FirstOrDefault(f => f.SpeciesId == preferredButNotRequiredSpeciesId);
            if (lastCatch == null || lastCatch.DateCaught.IsOlderThanMinutes(maxMinsOld))
            {
                lastCatch = fishCaught.FirstOrDefault();
            }

            if (lastCatch != null && lastCatch.DateCaught.IsOlderThanMinutes(maxMinsOld))
            {
                lastCatch = null;
            }

            
            if (lastCatch != null)
            {
                var method = await GetMethodString(lastCatch.FishingMethodId);
                var wayPoint = (await GetWayPointsAsync()).FirstOrDefault(w => w.WayPointId == lastCatch.WayPointId);

                return new NewFishOnViewModel()
                {
                    FishingMethod = method,
                    WayPointName = wayPoint?.Name,
                    Latitude = wayPoint?.Latitude,
                    Longitude = wayPoint?.Longitude
                };
            }

            return null;
        }
        

        private async Task<string> GetMethodString(int methodId)
        {
            var result = "";

            if (methodId > 0 && (await this.GetFishingMethodsAsync()) != null && _cachedMethods.Count > 0)
            {
                var method = _cachedMethods.FirstOrDefault(m => m.FishingMethodId == methodId);
                result = method?.Description;
            }

            return result;
        }

        public async Task SaveFishCaughtAsync(NewFishOnViewModel fishCaught)
        {
            var method = await ResolveFishingMethodAsync(fishCaught.FishingMethod);
            var lake = await ResolveLakeAsync(fishCaught.LakeName);
            var wayPoint = await ResolveWayPointAsync(fishCaught, lake);
          
            var fishOn = new Model.FishOn()
            {
                DateCaught = fishCaught.DateCaught.ToDate(),
                Image1File = fishCaught.Image1FileName,
                Image2File = fishCaught.Image2FileName,
                SpeciesId = fishCaught.SpeciesId,
                FishingMethodId = method.FishingMethodId,
                WeatherConditions = fishCaught.Conditions,
                MoonPercentage = fishCaught.MoonPhase,
                WaterTemp = fishCaught.WaterTemp.ToString(),
                WayPointId = wayPoint.WayPointId,
            };

            await _repoFactory.FishRepo.SaveFishOnAsync(fishOn);
        }

        private async Task<WayPoint> ResolveWayPointAsync(NewFishOnViewModel fishCaught, Lake lake)
        {
            var wayPoint = _cachedWayPoints.FirstOrDefault(w => w.Name == fishCaught.WayPointName);
            if (wayPoint == null)
            {
                wayPoint = new WayPoint()
                {
                    DateFirstCreated = fishCaught.DateCaught.ToDate(),
                    LakeId = lake.LakeId,
                    Latitude = fishCaught.Latitude.Value,
                    Longitude = fishCaught.Longitude.Value,
                    Name = fishCaught.WayPointName,
                    WayPointType = WayPoint.WayPointTypeEnumeration.FishOn
                };

                await _repoFactory.WayPointRepository.SaveAsync(wayPoint);
                _cachedWayPoints.Add(wayPoint);
            }

            return wayPoint;
        }

        private async Task<FishingMethod> ResolveFishingMethodAsync(string methodDescription)
        {
            var method = _cachedMethods.FirstOrDefault(m => m.Description == methodDescription);
            if (method == null)
            {
                method = new FishingMethod()
                {
                    Description = methodDescription
                };
                await _repoFactory.FishRepo.SaveFishingMethodAsync(method);
                _cachedMethods.Add(method);
            }

            return method;
        }

        private async Task<Lake> ResolveLakeAsync(string lakeName, double? latitude = null, double? longitude=null)
        {
            var lake =
                _cachedLakes.FirstOrDefault(l => l.LakeName.Equals(lakeName, StringComparison.CurrentCultureIgnoreCase));

            if (lake == null)
            {
                lake = new Lake()
                {
                    LakeName = lakeName,
                    Latitude = latitude,
                    Longitude = longitude
                };

                await _repoFactory.LakeRepo.SaveAsync(new List<Lake>() {lake});
                _cachedLakes.Add(lake);
            }

            return lake;
        }

        public async Task SaveWayPointProvisioningAsync(WayPoint wp)
        {
            
        }

        public async Task DeleteWayPointAsync(WayPoint wp)
        {
            
        }

        public async Task<List<Lake>> CreateNewLakesAsync(params string[] lakeNames)
        {
            await GetLakesAsync();

            var results = new List<Lake>();
            var newLakestoSave = new List<Lake>();
            
            foreach (var lakeName in lakeNames)
            {
                var existingLake = _cachedLakes.FirstOrDefault(l => String.Equals(l.LakeName, lakeName, StringComparison.CurrentCultureIgnoreCase));
                if (existingLake != null)
                {
                    results.Add(existingLake);
                }
                else
                {
                    var newLake = new Lake() {LakeName = lakeName};
                    _cachedLakes.Add(newLake);
                    newLakestoSave.Add(newLake);
                    results.Add(newLake);
                }
            }

            if (newLakestoSave.Count > 0)
            {
                await _repoFactory.LakeRepo.SaveAsync(newLakestoSave);
            }

            return results;
        }

        public async Task<List<AppSetting>>  GetAppSettingsAsync()
        {
            return await _repoFactory.AppSettingRepo.GetAppSettingsAsync();
        }

        public async Task DeleteLakeAsync(Lake lake)
        {
            
        }

        public async Task SaveLakeAsync(Lake lake)
        {
            
        }

        public async Task DeleteSpeciesAsync(Species species)
        {
            
        }

        public async Task SaveSpeciesAsync(Species species)
        {
            
        }

        public bool HasMapFilterEverBeenUsed { get; set; }
    }
}
