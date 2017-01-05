using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface ILakeDataService
    {
        Task<List<Lake>> GetLakesAsync();
        Task<Lake> GetLakeAsync(int lakeId);
        Task SaveAsync(Lake lake);
        Task DeleteAsync(int lakeId);
        Task<int> FindClosestLakeIdAsync(double latitude, double longitude);
        Task<List<Lake>> CreateNewLakesAsync(string[] lakeNames);
        Task<Lake> GetOrCreateLakeAsync(string lakeName);
    }

    public class LakeDataService : ILakeDataService
    {
        private ILakeRepository _lakeRepo;

        public LakeDataService()
        {
            _lakeRepo = new LakeRepository();
        }

        public LakeDataService(ILakeRepository lakeRepo)
        {
            _lakeRepo = lakeRepo;
        }

        public async Task<List<Lake>> GetLakesAsync()
        {
            return await _lakeRepo.GetLakesAsync();
        }

        public async Task<Lake> GetLakeAsync(int lakeId)
        {
            return await _lakeRepo.GetLakeAsync(lakeId);
        }

        public async Task<Lake> GetOrCreateLakeAsync(string lakeName)
        {
            return await _lakeRepo.GetLakeAsync(lakeName);
        }

        public async Task<List<Lake>> CreateNewLakesAsync(string[] lakeNames)
        {
            var currentLakes = await _lakeRepo.GetLakesAsync();
            var newLakes = new List<Lake>();

            foreach (string rawLakeName in lakeNames)
            {
                var lakeName = rawLakeName.Replace("Lake", "").Replace("lake", "").Trim();

                //Don't create duplicate lake names
                if (!currentLakes.Any(l => l.LakeName.Equals(lakeName, StringComparison.CurrentCultureIgnoreCase)) && lakeNames.Count(l => l.Equals(lakeName, StringComparison.CurrentCultureIgnoreCase)) <2 )
                {
                    newLakes.Add(new Lake() {LakeName = lakeName});
                }
            }

            await _lakeRepo.SaveAsync(newLakes);
            return await _lakeRepo.GetLakesAsync();
        }

        public async Task<int> FindClosestLakeIdAsync(double latitude, double longitude)
        {
            return -1;
        }

        public async Task SaveAsync(Lake lake)
        {

        }

        public async Task DeleteAsync(int lakeId)
        {

        }
    }
}
