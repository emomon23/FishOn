using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model
{
    //When the user goes out on the lake, they can set the water temp for the application
    //Any waypoints created will use the water temp that was set at the beginning of the session (up to X (8?) hours)
    public static class SessionData
    {
        private static int _currentWaterTemp;
        private static DateTime _sessionStartDateTime;
        private static int _currentLakeId;

        public static int CurrentWaterTemp
        {
            get { return _currentWaterTemp;  }
        }

        public static int CurrentLakeId { get { return _currentLakeId;} }

        public static double SessionAge_InMinutes
        {
            get { return (DateTime.Now - _sessionStartDateTime).TotalMinutes; }
        }

        public static void RestartSession(int? currentLakeId, int? currentWaterTemp)
        {
            _currentLakeId = currentLakeId ?? 0;
            _currentWaterTemp = currentWaterTemp ?? 0;
            _sessionStartDateTime = DateTime.Now;

        }
    }
}
