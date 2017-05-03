using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Repositories
{
    public interface IRepoFactory
    {
        IAppSettingRepo AppSettingRepo { get; set; }
        IFishRepository FishRepo { get; set; }
        ILakeRepository LakeRepo { get; set; }
        IWayPointRepository WayPointRepository { get; set; }
        ISpeciesRepository SpeciesRepository { get; set; }
    }

    public class RepoFactory : IRepoFactory
    {
        public IAppSettingRepo AppSettingRepo { get; set; } = new AppSettingRepo();
        public IFishRepository FishRepo { get; set; } = new FishRepository();
        public ILakeRepository LakeRepo { get; set; } = new LakeRepository();
        public IWayPointRepository WayPointRepository { get; set; } = new WayPointRepository();
        public ISpeciesRepository SpeciesRepository { get; set; } = new SpeciesRepository();
    }
}
