using System;
using System.Collections.Generic;
using System.Linq;
using FishOn.Model;
using Xamarin.Forms.Maps;

namespace FishOn.Services
{
    public interface ISessionDataService
    {
        Lake CurrentLake { get; set; }
        WeatherCondition CurrentWeatherCondition { get; set; }
        Position InitialPosition { get; set; }
        int CurrentWaterTemp { get; set; }
    }

    public class SessionDataService : ISessionDataService
    {
        private List<SessionData> _sessionData = new List<SessionData>();
        
        private object GetSessionData(string dataId, int maxAgeInMinutes, object valueIfNull = null)
        {
            var item = _sessionData.FirstOrDefault(s => s.SessionDataId == dataId);
            return item == null || item.CurrentValue_AgeInMinutes > maxAgeInMinutes? valueIfNull : item.DataValue;
        }

        private void SaveSessionData(string dataId, object value)
        {
            var item = _sessionData.FirstOrDefault(s => s.SessionDataId == dataId);
            if (item == null)
            {
                item = new SessionData() {SessionDataId = dataId};
                _sessionData.Add(item);
            }

            item.DataValue = value;
            item.DateLastUpdated = DateTime.Now;
        }

        public Lake CurrentLake
        {
            get { return (Lake)GetSessionData("currentLakeId",500); }
            set { SaveSessionData("currentLakeId", value);}
        }

        public WeatherCondition CurrentWeatherCondition
        {
            get
            {
                return (WeatherCondition)GetSessionData("currentWeatherConditions", 60); ;
            }
            set
            {
                SaveSessionData("currentWeatherConditions", value);
            }
        }

        public Position InitialPosition
        {
            get
            {
                var postion = GetSessionData("initialPosition", 60 * 8);
                if (postion != null)
                {
                    return (Position) postion;
                }

                return new Position(0, 0);
            }
            set
            {
                SaveSessionData("initialPosition", value);
            }
        }

        public int CurrentWaterTemp
        {
            get { return (int)GetSessionData("currentWaterTemp", 500, 0); }
            set { SaveSessionData("currentWaterTemp", value);}
        }
    }
}
