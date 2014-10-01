using System;
using System.IO;
using System.Threading.Tasks;
using MonoTouch.Foundation;
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

//        public async Task<bool> SaveImageToDisk(ImageSource imgSrc, string name)
//        {
//            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//            var fullFilePath = Path.Combine(documentsDirectory, name);
//
//            var renderer = new StreamImagesourceHandler();
//            var photo = await renderer.LoadImageAsync(imgSrc);
//            var imgData = photo.AsJPEG();
//
//            if (File.Exists(fullFilePath))
//            {
//                return false;
//            }
//
//            NSError err = null;
//            if (!imgData.Save(fullFilePath, false, out err))
//            {
//                throw new Exception(err.LocalizedDescription);
//            }
//            return true;
//        }

        public ImageSource LoadImageFromDisk(string name, string folder = null)
        {
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

