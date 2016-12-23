using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public class SpeciiesRepository
    {
        public interface ISpeciesRepository
        {
            Task<List<Species>> GetSpecies();
            Task<List<Species>> GetSpecies(int lakeId);
            Task Save(Species species);
            Task Delete(int speciesId);
        }

        public class SpeciesRepository : ISpeciesRepository
        {
            private IFishOnHttpRepository _fishOnHttp;

            public SpeciesRepository(IFishOnHttpRepository fishOnHttp)
            {
                _fishOnHttp = fishOnHttp;
            }

            public async Task<List<Species>> GetSpecies()
            {
                return null;
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
}
