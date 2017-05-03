using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model.ViewModel
{
    public class WayPointPhotoAlbumViewModel
    {
        public int WayPointId { get; set; }
        public string WayPointName { get; set; }

        public List<WayPointImageViewModel> Images { get; set; }
    }

    public class WayPointImageViewModel
    {
        public string ImageFilePath { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
