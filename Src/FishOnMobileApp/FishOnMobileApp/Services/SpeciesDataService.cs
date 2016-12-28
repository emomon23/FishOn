using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Repositories;

namespace FishOn.Services
{
    public interface ISpeciesDataService
    {
        Task<List<Species>> GetSpecies();
        Task<List<Species>> GetSpecies(int lakeId);
        Task Save(Species species);
        Task Delete(int speciesId);
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

        public async Task<List<Species>> GetSpecies()
        {
            return await _speciesRepository.GetSpecies();
        }

        public async Task<List<Species>> GetSpecies(int lakeId)
        {
            return null;
        }

        public async Task Save(Species species)
        {

        }

        public async Task Delete(int speciesId)
        {

        }
    }
}
