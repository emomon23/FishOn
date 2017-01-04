using System.IO;
using Windows.Storage;
using FishOn.PlatformInterfaces;
using FishOnMobileApp.WinPhone;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace FishOnMobileApp.WinPhone
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}
