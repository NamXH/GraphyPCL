using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(GraphyPCL.iOS.FileAccess))]
namespace GraphyPCL.iOS
{
    public class FileAccess : IFileAccess
    {
        public bool Exists(string filename)
        {
            var filePath = GetFilePath(filename);

            if (File.Exists(filePath))
            {
                FileInfo finf = new FileInfo(filePath);
                return finf.Length > 0;
            }
            else
            {
                return false;
            }
        }

        public string FullPath(string filename)
        {
            if (String.IsNullOrEmpty(filename))
            {
                return null;
            }
            
            var filePath = GetFilePath(filename);

            if (!File.Exists(filePath))
            {
                return null;
            }

            return filePath;
        }

        static string GetFilePath(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return filePath;
        }

        public void WriteStream(string filename, Stream streamIn)
        {
            var filePath = GetFilePath(filename);
            using (var fs = File.Create(filePath))
            {
                streamIn.CopyTo(fs); 
            }
        }
    }
}