using System.Threading.Tasks;
using FishOn.Model;

namespace FishOn.Repositories
{
    public interface IWeatherRepository
    {
        Task SaveAsync(WeatherCondition weatherCondition);
    }

    public class WeatherRepository : BaseRepository, IWeatherRepository
    {
        public WeatherRepository(IFishOnHttpRepository fishOnHttp = null) : base(fishOnHttp) { }

        public async Task SaveAsync(WeatherCondition weatherCondition)
        {
            if (weatherCondition == null)
            {
                return;
            }
            var db = await GetDB();
            await db.SaveWeatherConditionAsync(weatherCondition);
        }
    }
}
