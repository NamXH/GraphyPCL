using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(GraphyPCL.iOS.PhotoService))]
namespace GraphyPCL.iOS
{
    public class PhotoService : IPhotoService
    {
        public PhotoService()
        {
        }

        public async Task<bool> SaveImageToDisk(ImageSource imgSrc, string name)
        {
            if ((imgSrc == null) || (String.IsNullOrEmpty(name)))
            {
                // Need better exception!!
                throw new ArgumentException("Arguments are null or empty.");
            }

            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fullFilePath = Path.Combine(documentsDirectory, name);

            if (File.Exists(fullFilePath))
            {
                return false;
            }

            var renderer = new StreamImagesourceHandler();
            var photo = await renderer.LoadImageAsync(imgSrc);
            var imgData = photo.AsJPEG();

            NSError err = null;
            if (!imgData.Save(fullFilePath, false, out err))
            {
                throw new Exception(err.LocalizedDescription);
            }
            return true;
        }

        public ImageSource LoadImageFromDisk(string name, string folder = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                return null;
            }

            folder = (folder) ?? Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fullFilePath = Path.Combine(folder, name);

            if (!File.Exists(fullFilePath))
            {
                return null;
            }

            return ImageSource.FromFile(fullFilePath);
        }
    }
}

