using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface ISpeciesRepository
    {
        Task<List<Species>> GetSpeciesAsync();
        Task<Species> GetSpeciesAsync(int speciesId);
        Task SaveAsync(Species species);
        Task DeleteAvailableAsync(Species species);
    }

    public class SpeciesRepository : BaseRepository, ISpeciesRepository
    {
        private List<Species> _cachedSpecies;

        public SpeciesRepository(IFishOnHttpRepository fishOnHttp = null, IFishOnDataContext dataContext = null)
            : base(fishOnHttp, dataContext)
        {
        }

        public async Task<List<Species>> GetSpeciesAsync()
        {
            if (_cachedSpecies == null)
            {
                _cachedSpecies = await (await GetDB()).GetSpeciesAsync();
            }

            return _cachedSpecies;
        }

        public async Task<Species> GetSpeciesAsync(int speciesId)
        {
            await GetSpeciesAsync();
            return _cachedSpecies.SingleOrDefault(s => s.SpeciesId == speciesId);
        }

        public async Task DeleteAvailableAsync(Species species)
        {
            var db = await GetDB();
            db.DeleteAvailableAsync(species);

            _cachedSpecies.Remove(species);
        }

        public async Task SaveAsync(Species species)
        {
            var db = await GetDB();
            var isNewSpecies = species.SpeciesId == 0;
            db.SaveSpeciesAsync(species);

            if (isNewSpecies)
            {
                _cachedSpecies.Add(species);
            }
        }
    }
}