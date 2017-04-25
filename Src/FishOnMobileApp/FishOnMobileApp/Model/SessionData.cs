using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FishOn.Model
{
    public class SessionData
    {
        public SessionData()
        {
            DateCreated = DateTime.Now;
            DateLastUpdated = DateCreated;
        }

        public string SessionDataId { get; set; }    
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public object DataValue { get; set; }

        public double CurrentValue_AgeInMinutes
        {
            get { return (DateTime.Now - DateLastUpdated).TotalMinutes; }
        }

        public double CurrentValue_AgeInDays
        {
            get { return (DateTime.Now - DateLastUpdated).TotalDays; }
        }
    }
}
