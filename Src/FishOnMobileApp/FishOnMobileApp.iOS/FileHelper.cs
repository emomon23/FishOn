using System;
using System.IO;
using FishOn.PlatformInterfaces;
using FishOnMobileApp.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace FishOnMobileApp.iOS
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}
