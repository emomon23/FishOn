using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model.ViewModel
{
    public class SpeciesCaughtViewModel
    {
        public string SpeciesName { get; set; }
        public List<Model.FishOn> FishCaught { get; set; }
    }
}
