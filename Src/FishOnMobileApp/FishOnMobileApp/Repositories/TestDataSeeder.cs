using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Services;
using FishOn = FishOn.Model.FishOn;

namespace FishOn.Repositories
{
    public static class TestDataSeeder
    {
        private static IWayPointDataService _wpService = new WayPointDataService();
        private static ISpeciesDataService _speciesDataService = new SpeciesDataService();
        private static ILakeDataService _lakeDataService = new LakeDataService();
        private static List<Species> _specieses = null;
        private static Random _rnd = new Random(DateTime.Now.Millisecond);

        public async static Task SeedTestDataAsync()
        {
            if (await HasDBAlreadyBeenSeedWithTestDataAsync())
            {
                return;
            }

            _specieses = await _speciesDataService.GetSpeciesAsync();
            var lakes = await CreateLakesAsync("Tonka", "Lotus", "Waconia", "Leech", "Pine Mtn", "Auburn", "Pleasent");
            var halstedWP = await CreateWayPointAsync(44.908303482587, -93.5001525712633, "Halsted Bay", lakes[0],
                    WayPoint.WayPointTypeEnumeration.FishOn);

            var carsonWP = await CreateWayPointAsync(44.9891699798566, -93.3829020817899, "Carsons", lakes[1]);
            var stpaul = await CreateWayPointAsync(44.9497604439405, -93.1458829886985, "stPault", lakes[0]);

            await CreateWayPointAsync(44.9593604339201, -93.2423629878985, "GrandmasBay", lakes[0]);
            await CreateWayPointAsync(44.9293604332701, -93.1553533878985, "Laffeyette", lakes[1 ]);

            await CreateFishOnAsync(halstedWP, "Walleye", "Trolling", "Right after wind picked up");
            await CreateFishOnAsync(halstedWP, "Crappie", "", "");
            await CreateFishOnAsync(halstedWP, "Walleye", "", "");
            await CreateFishOnAsync(halstedWP, "Walleye", "", "");
            await CreateFishOnAsync(halstedWP, "Lg M Bass", "", "");
            await CreateFishOnAsync(halstedWP, "Sm M Bass", "", "");
            await CreateFishOnAsync(halstedWP, "Muskie", "", "");
            await CreateFishOnAsync(halstedWP, "Catfish", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");
            await CreateFishOnAsync(halstedWP, "Pike", "", "");

            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");

            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Pike", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");
            await CreateFishOnAsync(carsonWP, "Sunny", "", "");


        }

        private async  static  Task<bool> HasDBAlreadyBeenSeedWithTestDataAsync()
        {
            return (await _lakeDataService.GetLakesAsync()).Any(l => l.LakeName == "Pleasent");
        }

        private  async  static Task CreateFishOnAsync(WayPoint wp, string speciesName, string method, string notes)
        {
            var species = _specieses.SingleOrDefault(s => s.Name == speciesName);

            var fo = new Model.FishOn()
            {
                DateCaught = GetRandomDate(),
                SpeciesId =  species.SpeciesId,
                Species =  species,
                WaterTemp = "52",
                WayPointId =  wp.WayPointId,
                WayPoint =  wp,
                Method = method,
                Note =  notes,
                WeatherCondition =  new WeatherCondition()
                {
                    AirTemp = _rnd.Next(0, 94),
                    BerometricPressure = (double)_rnd.Next(20, 70),
                    HumidityPercent = _rnd.Next(60,90),
                    Moon_Label = "Full",
                    Moon_Age = 14,
                    Moon_IlluminationPercent = 100,
                    DewPoint = _rnd.Next(25,100),
                    Visibility = 25
                }
            };

            wp.FishCaught.Add(fo);

            await _wpService.SaveWayPointProvisioningAsync(wp);
        }

        private async static Task<WayPoint> CreateWayPointAsync(double lat, double longitude, string name, Lake lake,
            WayPoint.WayPointTypeEnumeration wpType = WayPoint.WayPointTypeEnumeration.FishOn)
        {
            WayPoint wp = new WayPoint()
            {
                LakeId = lake.LakeId,
                Name = name,
                Latitude = lat,
                Longitude = longitude,
                WayPointType = wpType,
                Lake = lake,
            };

            await _wpService.SaveWayPointProvisioningAsync(wp);
            return wp;
        }

        private async static Task<List<Lake>> CreateLakesAsync(params string [] lakes)
        {
            var result = await _lakeDataService.CreateNewLakesAsync(lakes);
            return result;
        }

        private static DateTime GetRandomDate()
        {
            var daysBakc = _rnd.Next(0, 730);
            var minsBack = _rnd.Next(0, 1500);

            var result = DateTime.Now.AddDays((-1 * daysBakc)).AddMinutes((-1 * minsBack));
            return result;
        }
    }
}
