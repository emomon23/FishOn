using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using Xamarin.Forms;
using Model = FishOn.Model;

namespace FishOn.Repositories
{
    public interface IFishOnDataContext
    {
        Task InitializeAsync();
        Task<List<Species>> GetSpeciesAsync();
        Task SaveNewLakesAsync(List<Lake> newLakes);
        Task<List<Lake>> GetLakesAsync();
    }

    public class FishOnDataContext : IFishOnDataContext
    {
        private SQLiteAsyncConnection _db;
        private readonly ISQLite _sqLite;
        private static IFishOnDataContext _singletonInstance;

        #region Constructor_Initialization

        public FishOnDataContext() : this(DependencyService.Get<ISQLite>()){}

        public FishOnDataContext(ISQLite sqlLite)
        {
            _db = sqlLite.GetAsyncConnection();
            _sqLite = sqlLite;
        }

        public static async Task<IFishOnDataContext> GetInstanceAsync()
        {
            if (_singletonInstance == null)
            {
                _singletonInstance = new FishOnDataContext();
                await _singletonInstance.InitializeAsync();
            }

            return _singletonInstance;
        }
        
        public async Task  InitializeAsync()
        {
            _sqLite.DeleteDatabase();

            if (!_sqLite.DoesDBExist)
            {
                await CreateDatabaseAsync();
                await SeedDatabaseAsync();
            }
        }

        #endregion

        #region Species

        public async Task<List<Species>> GetSpeciesAsync()
        {
            return await _db.Table<Species>().ToListAsync();
        }

        public async Task UpdateSpecies(IList<Species> specieses)
        {
            await _db.UpdateAllAsync(specieses);
        }

        public async Task CreateNewSpecies(Species species)
        {
            await _db.InsertAsync(species);
        }

        #endregion

        #region Lakes
        
        public async Task SaveNewLakesAsync(List<Lake> newLakes)
        {
            foreach (var lake in newLakes)
            {
                var result = await _db.InsertAsync(lake);
            }
        }

        public async Task<List<Lake>>  GetLakesAsync()
        {
            return await _db.Table<Lake>().ToListAsync();
        }

        #endregion

        #region WayPoints

        

        #endregion

        #region DBCreation Seeding

        private async Task CreateDatabaseAsync()
        {
            await _db.CreateTableAsync<Species>();
            await _db.CreateTableAsync<WayPoint>();
            await _db.CreateTableAsync<Lake>();
            await _db.CreateTableAsync<Model.FishOn>();
            await _db.CreateTableAsync<WeatherCondition>();
            await _db.CreateTableAsync<FishingLure>();
        }

        private async Task SeedDatabaseAsync()
        {
              await _db.InsertAsync(new Species() { Description = "", Name = "Walleye", DisplayOrder = 10});
              await _db.InsertAsync(new Species() { Description = "Northern Pike 'Snake'", Name = "Pike", DisplayOrder = 20});
              await _db.InsertAsync(new Species() { Description = "Panfish", Name = "Sunny", DisplayOrder = 30});
              await _db.InsertAsync(new Species() { Description = "Panfish", Name = "Crappie", DisplayOrder = 40});
              await _db.InsertAsync(new Species() { Description = "", Name = "Lg M Bass.", DisplayOrder = 50});
              await _db.InsertAsync(new Species() { Description = "", Name = "Sm M Bass", DisplayOrder = 60});
              await _db.InsertAsync(new Species() { Description = "", Name = "Muskie", DisplayOrder = 70});
              await _db.InsertAsync(new Species() { Description = "", Name = "Catfish", DisplayOrder = 80});
        }

        #endregion
    }
}
