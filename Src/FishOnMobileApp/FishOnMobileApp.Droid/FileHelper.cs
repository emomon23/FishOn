using System;
using System.IO;
using FishOn.PlatformInterfaces;
using FishOnMobileApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace FishOnMobileApp.Droid
{
    
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public string GetRootFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        public void WriteAllText(string text, string path)
        {
            File.WriteAllText(path, text);
        }

        public void DeleteFile(string fileName)
        {
            if (Exists(fileName))
            {
                File.Delete(fileName);
            }
            else
            {
                var path = GetLocalFilePath(fileName);
                if (Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteStream(Stream stream, string path)
        {
            using (var fileStream = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();
            }
        }

        public Stream ReadStream(string path)
        {
            MemoryStream ms = new MemoryStream();

            using (FileStream file = new FileStream("file.bin", FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
            }

            return ms;
        }

        public void WriteAllBytes(byte[] bytes, string path)
        {
            File.WriteAllBytes(path, bytes);
        }

        public Byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}