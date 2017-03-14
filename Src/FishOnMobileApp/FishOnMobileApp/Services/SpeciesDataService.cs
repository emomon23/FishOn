using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface ISpeciesDataService
    {
        Task<List<Species>> GetSpeciesAsync();
        Task<List<Species>> GetSpeciesAsync(int lakeId);
        Task SaveAsync(Species species);
        Task DeleteAsync(Species species);
    }

    public class SpeciesDataService : ISpeciesDataService
    {
        private ISpeciesRepository _speciesRepository;

        public SpeciesDataService()
        {
            _speciesRepository = new SpeciesRepository();
        }

        public SpeciesDataService(ISpeciesRepository speciesRepository)
        {
            _speciesRepository = speciesRepository;
        }

        public async Task<List<Species>> GetSpeciesAsync()
        {
            return await _speciesRepository.GetSpeciesAsync();
        }

        public async Task<List<Species>> GetSpeciesAsync(int lakeId)
        {
            return null;
        }

        public async Task SaveAsync(Species species)
        {
            await _speciesRepository.SaveAsync(species);
        }

        public async Task DeleteAsync(Species species)
        {
            await _speciesRepository.DeleteAvailableAsync(species);
        }
    }
}
