using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.PlatformInterfaces
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
        string GetRootFolder();
        bool Exists(string fileName);
        void WriteAllText(string text, string path);
        string ReadAllText(string path);
        void WriteStream(Stream stream, string path);
        Stream ReadStream(string path);
        void WriteAllBytes(byte[] bytes, string path);
        Byte[] ReadAllBytes(string path);
        void DeleteFile(string fileName);
    }
}
