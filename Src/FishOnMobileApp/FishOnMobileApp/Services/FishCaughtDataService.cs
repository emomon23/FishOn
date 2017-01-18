﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface IFishCaughtDataService
    {
        Task UpdateFishCaughtAsync(Model.FishOn fishCaught);
        Task<List<FishingLure>> GetAvailableLuresAsync();
        Task DeleteFishCaughtAsync(Model.FishOn fishCaught);
    }

    public class FishCaughtDataService : IFishCaughtDataService
    {
        private List<WayPoint> _cachedWayPoints = null;
        private IWayPointRepository _wayPointRepository;
        private ILakeRepository _lakeRepository;
        private IWeatherService _weatherService;
        private IFishRepository _fishRepository;
        private IWeatherRepository _weatherRepository;
        private ISpeciesRepository _speciesRepository;

        public FishCaughtDataService()
        {
            _wayPointRepository = new WayPointRepository();
            _lakeRepository = new LakeRepository();
            _weatherService = new WeatherService();
            _fishRepository = new FishRepository();
            _weatherRepository = new WeatherRepository();
            _speciesRepository = new SpeciesRepository();
        }

        public FishCaughtDataService(IWayPointRepository wayPointRepository, ILakeRepository lakeRepository, IWeatherService weatherService, IFishRepository fishRepo, IWeatherRepository weatherRepo, ISpeciesRepository speciesRepository)
        {
            _wayPointRepository = wayPointRepository;
            _lakeRepository = lakeRepository;
            _weatherService = weatherService;
            _weatherRepository = weatherRepo;
            _fishRepository = fishRepo;
            _speciesRepository = speciesRepository;
        }

        public async Task UpdateFishCaughtAsync(Model.FishOn fishCaught)
        {
            if (fishCaught.Lure.IsValid)
            {
                await _fishRepository.SaveLureAsync(fishCaught.Lure);
                fishCaught.FishingLureId = fishCaught.Lure.FishingLureId;
            }

            await _fishRepository.SaveFishOnAsync(fishCaught);
            await _weatherRepository.SaveAsync(fishCaught.WeatherCondition);
        }

        public async Task DeleteFishCaughtAsync(Model.FishOn fishCaught)
        {
            await _fishRepository.DeleteFishCaughtAsync(fishCaught);
        }

        public async Task<List<FishingLure>> GetAvailableLuresAsync()
        {
           return await _fishRepository.GetAvailableLuresAsync();
        }
    }
}