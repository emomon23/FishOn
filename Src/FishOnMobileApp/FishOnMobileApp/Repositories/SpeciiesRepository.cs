using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface ISpeciesRepository
    {
        Task<List<Species>> GetSpecies();
        Task<List<Species>> GetSpecies(int lakeId);
        Task Save(Species species);
        Task Delete(int speciesId);
    }

    public class SpeciesRepository : BaseRepository, ISpeciesRepository
    {
        public SpeciesRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp)
        {
        }

        public async Task<List<Species>> GetSpecies()
        {
            return new List<Species>()
            {
                new Species()
                {
                    SpeciesId = 1,
                    Description = "Panfish",
                    Name = "Sunny"
                },
                new Species()
                {
                    SpeciesId = 2,
                    Description = "Panfish",
                    Name = "Crappie"
                },
                new Species()
                {
                    SpeciesId = 3,
                    Description = "Panfish",
                    Name = "Perch"
                },
                new Species()
                {
                    SpeciesId = 4,
                    Description = "Fun to catch, wouldn't eat though",
                    Name = "Bass"
                },
                new Species()
                {
                    SpeciesId = 5,
                    Description = "Norther Pike.  Preditor fish",
                    Name = "Pike"
                },
                new Species()
                {
                    SpeciesId = 6,
                    Description = "Minnesota's State Fish",
                    Name = "Walleye"
                },
                new Species()
                {
                    SpeciesId = 1,
                    Description = "A tough to catch trophy speciies",
                    Name = "Muskie"
                },
            };
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