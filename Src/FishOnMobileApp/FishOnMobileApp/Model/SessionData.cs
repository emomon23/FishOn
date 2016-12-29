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
        private static int currentWaterTemp;
        private static DateTime waterTempSetDateTime;

        public static int CurrentWaterTemp
        {
            get { return currentWaterTemp;  }
            set
            {
                currentWaterTemp = value;
                waterTempSetDateTime = DateTime.Now;
            }
        }

        public static double AgeOfWaterTempSetting_InMinutes
        {
            get { return (DateTime.Now - waterTempSetDateTime).TotalMinutes; }
        }
    }
}
