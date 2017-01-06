using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FishOn.PlatformInterfaces
{
   
    public delegate Task GetCurrentPositionCallBackDelegateAsync(Position? position, string errorMessage);

    public interface IFishOnCurrentLocationService
    {
        void GetCurrentPosition(GetCurrentPositionCallBackDelegateAsync callBack);
    }
}
