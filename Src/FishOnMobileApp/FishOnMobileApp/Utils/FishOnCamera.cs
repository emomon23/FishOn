using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.PlatformInterfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace FishOn.Utils
{
    public class FishOnCamera
    {
        PlatformInterfaces.IFileHelper fileSystem = DependencyService.Get<IFileHelper>();

        public async Task Initialize()
        {
            await CrossMedia.Current.Initialize();
        }

        public static void DeleteImages(params string[] imageNames)
        {
            IFileHelper fileSystem = DependencyService.Get<IFileHelper>();
            foreach (var imageFile in imageNames)
            {
                if (imageFile.IsNotNullOrEmpty())
                {
                    fileSystem.DeleteFile(imageFile);
                }
            }
        }

        public async Task<CameraResult> TakePictureAsync()
        {
            var media = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                SaveToAlbum = false
            });

            string fileName = $"FishOn_{Guid.NewGuid().ToString().Substring(0, 8)}";
            string path = GetPath(fileName);
            fileSystem.WriteStream(media.GetStream(), path);


            return new CameraResult()
            {
                FileName = fileName,
                FullPath = path,
                Media = media
            };
        }

        public async Task<Stream> ReadPicutreAsync(string picNameNoExtension)
        {
            return fileSystem.ReadStream(GetPath(picNameNoExtension));
        }

        private string GetPath(string fileName)
        {
            return fileSystem.GetLocalFilePath(fileName + ".jpg");
        }

        public bool IsCameraAvailable => CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
        public bool IsPhotoPickingSupported => CrossMedia.Current.IsPickPhotoSupported;
    }

    public class CameraResult
    {
        public MediaFile Media { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
    }
}
