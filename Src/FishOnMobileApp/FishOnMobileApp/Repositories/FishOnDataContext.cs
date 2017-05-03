using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using SQLite.Net.Async;
using Xamarin.Forms;

namespace FishOn.Repositories
{
    public interface IFishOnDataContext
    {
        Task InitializeAsync();
        Task<List<Species>> GetSpeciesAsync();
        Task SaveNewLakesAsync(List<Lake> newLakes);
        Task<List<Lake>> GetLakesAsync();
        Task<Lake> GetLakeAsync(string lakeName);
        Task<List<FishingMethod>> GetFishingMethodsAsync();
        Task SaveFishingMethodAsync(FishingMethod fishingMethod);
        Task<Lake> GetLakeAsync(int lakeId);
        Task<List<WayPoint>> GetWayPointsAsync(int? lakeId = null);
        Task SaveWayPointAsync(WayPoint wp);
        Task<WayPoint> GetWayPointAsync(double latitude, double longitude);
        Task SaveFishOnAsync(Model.FishOn fishCaught);
        Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId);
        Task<List<Model.FishOn>> GetFishCaughtOnLure(int lureId);
        Task<List<Model.FishOn>> GetFishCaught();
        Task DeleteWayPointAsync(WayPoint wayPoint);
        Task<List<AppSetting>> GetAppSettingsAsync();
        Task SaveAppSettingAsync(AppSetting setting);
        Task DeleteFishCaughtAsync(Model.FishOn fishCaught);
        Task DeleteLakeAsync(Lake lake);
        Task SaveSpeciesAsync(Species species);
        Task DeleteAvailableAsync(Species species);

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
           // _sqLite.DeleteDatabase();

            if (!_sqLite.DoesDBExist)
            {
                await CreateDatabaseAsync();
                await SeedDatabaseAsync();
            }

            await TestDataSeeder.SeedTestDataAsync();
        }

        #endregion

        #region AppSettings

        public async Task<List<AppSetting>> GetAppSettingsAsync()
        {
            var result = await _db.Table<AppSetting>().ToListAsync();
            return result;
        }

        public async Task SaveAppSettingAsync(AppSetting setting)
        {
            await _db.UpdateAsync(setting);
        }

        #endregion

        #region Species

        public async Task<List<Species>> GetSpeciesAsync()
        {
            return await _db.Table<Species>().Where(s => s.IsDeleted == false).ToListAsync();
        }

        public async Task UpdateSpecies(IList<Species> specieses)
        {
            await _db.UpdateAllAsync(specieses);
        }

        public async Task CreateNewSpecies(Species species)
        {
            await _db.InsertAsync(species);
        }

        public async Task DeleteAvailableAsync(Species species)
        {
            //See if we can physically delete the species
            var speciesHasBeenCaught = (await _db.Table<Model.FishOn>().Where(f => f.SpeciesId == species.SpeciesId).ToListAsync()).Any();

            if (speciesHasBeenCaught)
            {
                species.IsDeleted = true;
                await SaveSpeciesAsync(species);
            }
            else
            {
                await _db.DeleteAsync(species);
            }
        }

        public async Task SaveSpeciesAsync(Species species)
        {
            if (species.SpeciesId == 0)
            {
                await CreateNewSpecies(species);
                return;
            }

            _db.UpdateAsync(species);

        }

        #endregion

        #region Lakes

        public async Task SaveNewLakesAsync(List<Lake> newLakes)
        {
            foreach (var lake in newLakes)
            {
                if (lake.LakeId == 0)
                {
                    await _db.InsertAsync(lake);
                }
                else
                {
                    await _db.UpdateAsync(lake);
                }
            }
        }

        public async Task DeleteLakeAsync(Lake lake)
        {
            await _db.DeleteAsync(lake);
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
        
        #region Fish Caught / Fishing Methods

        public async Task SaveFishOnAsync(Model.FishOn fishCaught)
        {
            if (fishCaught.FishOnId == 0)
            {
                await _db.InsertAsync(fishCaught);
            }
            else
            {
                await _db.UpdateAsync(fishCaught);
            }
        }



        public async Task DeleteFishCaughtAsync(Model.FishOn fishCaught)
        {
            await _db.DeleteAsync(fishCaught);
        }

        public async Task<List<Model.FishOn>> GetFishCaughtAtWayPointAsync(int wayPointId)
        {
            return await _db.Table<Model.FishOn>().Where(f => f.WayPointId == wayPointId).ToListAsync();
        }

        public async Task<List<Model.FishOn>> GetFishCaughtOnLure(int lureId)
        {
            return await _db.Table<Model.FishOn>().Where(f => f.FishingLureId == lureId).ToListAsync();
        }

        public async Task<List<Model.FishOn>> GetFishCaught()
        {
            return await _db.Table<Model.FishOn>().ToListAsync();
        }

        public async Task<List<FishingMethod>> GetFishingMethodsAsync()
        {
            return await _db.Table<Model.FishingMethod>().ToListAsync();
        }

        public async Task SaveFishingMethodAsync(FishingMethod fishingMethod)
        {
            if (fishingMethod.FishingMethodId == 0)
            {
                await _db.InsertAsync(fishingMethod);
            }
            else
            {
                await _db.UpdateAsync(fishingMethod);
            }
        }

        #endregion

        #region DBCreation Seeding

        private async Task CreateDatabaseAsync()
        {
            await _db.CreateTableAsync<AppSetting>();
            await _db.CreateTableAsync<Species>();
            await _db.CreateTableAsync<WayPoint>();
            await _db.CreateTableAsync<Lake>();
            await _db.CreateTableAsync<Model.FishOn>();
            await _db.CreateTableAsync<FishingMethod>();

        }

        private async Task SeedDatabaseAsync()
        {
              await _db.InsertAsync(new Species() { Description = "", Name = "Walleye", DisplayOrder = 10, IsAvailableOnCatchList = true, imageIcon = "walleye.png",DisplaySpeciesOnLakeMap = true});
              await _db.InsertAsync(new Species() { Description = "Northern Pike 'Snake'", Name = "Pike", DisplayOrder = 20, IsAvailableOnCatchList = true, imageIcon = "pike.png", DisplaySpeciesOnLakeMap = true });
              await _db.InsertAsync(new Species() { Description = "Panfish", Name = "Sunny", DisplayOrder = 30, IsAvailableOnCatchList = true, imageIcon = "sunfish.png" ,DisplaySpeciesOnLakeMap = true });
              await _db.InsertAsync(new Species() { Description = "Panfish", Name = "Crappie", DisplayOrder = 40, IsAvailableOnCatchList = true, imageIcon = "crappie.png", DisplaySpeciesOnLakeMap = true });
              await _db.InsertAsync(new Species() { Description = "", Name = "Lg M Bass", DisplayOrder = 50, IsAvailableOnCatchList = true, imageIcon = "largemouth.png" ,DisplaySpeciesOnLakeMap = true });
              await _db.InsertAsync(new Species() { Description = "", Name = "Sm M Bass", DisplayOrder = 60, IsAvailableOnCatchList = true, imageIcon = "smallmouth.png",DisplaySpeciesOnLakeMap = true });
              await _db.InsertAsync(new Species() { Description = "", Name = "Muskie", DisplayOrder = 70, IsAvailableOnCatchList = true, imageIcon = "muskie.gif", DisplaySpeciesOnLakeMap = true });
              await _db.InsertAsync(new Species() { Description = "", Name = "Catfish", DisplayOrder = 80, IsAvailableOnCatchList = true, imageIcon = "catfish.gif", DisplaySpeciesOnLakeMap = true });

            await _db.InsertAsync(new AppSetting() {SettingName = "MapFilterHasBeenUsed", Value = false.ToString()});
        }

        #endregion
    }
}
