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
        Task<Lake> GetLakeAsync(string lakeName);
        Task<Lake> GetLakeAsync(int lakeId);
        Task<List<WayPoint>> GetWayPointsAsync(int? lakeId = null);
        Task SaveWayPointAsync(WayPoint wp);
        Task<WayPoint> GetWayPointAsync(double latitude, double longitude);
        Task SaveFishOnAsync(Model.FishOn fishCaught);
        Task SaveNewLureAsync(FishingLure lure);
        Task SaveWeatherConditionAsync(WeatherCondition wc);
        Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId);
        Task DeleteWayPointAsync(WayPoint wayPoint);
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
            //_sqLite.DeleteDatabase();

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

        public async Task<Lake> GetLakeAsync(string lakeName)
        {
            var lakes = await _db.Table<Lake>()
                    .Where(l => l.LakeName == lakeName)
                    .ToListAsync();

            return lakes != null && lakes.Count == 1 ? lakes[0] : null;
        }

        public async Task<Lake> GetLakeAsync(int lakeId)
        {
            var lakes = await _db.Table<Lake>()
                    .Where(l => l.LakeId == lakeId)
                    .ToListAsync();

            return lakes != null && lakes.Count == 1 ? lakes[0] : null;
        }

        public async Task<List<Lake>>  GetLakesAsync()
        {
            return await _db.Table<Lake>().ToListAsync();
        }

        #endregion

        #region WayPoints

        public async Task<WayPoint> GetWayPointAsync(double latitude, double longitude)
        {
            return
                await _db.Table<WayPoint>()
                    .Where(w => w.Latitude == latitude && w.Longitude == longitude)
                    .FirstOrDefaultAsync();
        }

        public async Task<List<WayPoint>> GetWayPointsAsync(int? lakeId = null)
        {
            if (lakeId.HasValue)
            {
                return await _db.Table<WayPoint>().Where(w => w.LakeId == lakeId.Value).ToListAsync();
            }

            return await _db.Table<WayPoint>().ToListAsync();
        }

        public async Task DeleteWayPointAsync(WayPoint wayPoint)
        {
            var id = wayPoint.WayPointId;
            await _db.DeleteAsync(wayPoint);

            var fishCaught = await this.GetFishCaughtAtWayPointAsync(id);
            foreach (var fish in fishCaught)
            {
                var weather = _db.Table<WeatherCondition>().Where(w => w.WeatherConditionId == fish.FishOnId);
                await _db.DeleteAsync(weather);
                await _db.DeleteAsync(fish);
            }

        }

        public async Task SaveWayPointAsync(WayPoint wp)
        {
            if (wp.WayPointId == 0)
            {
                await _db.InsertAsync(wp);
            }
            else
            {
                await _db.UpdateAsync(wp);
            }
        }


        #endregion

        #region WeatherConditions

        public async Task SaveWeatherConditionAsync(WeatherCondition wc)
        {
            _db.InsertAsync(wc);
        }

        #endregion
        
        #region Fish Caught / Fishing Lures

        public async Task SaveFishOnAsync(Model.FishOn fishCaught)
        {
            await _db.InsertAsync(fishCaught);
        }

        public async Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId)
        {
            return await _db.Table<Model.FishOn>().Where(f => f.WayPointId == wayPointId).ToListAsync();
        }

        public async Task SaveNewLureAsync(FishingLure lure)
        {
            await _db.InsertAsync(lure);
        }
        
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
