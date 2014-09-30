using System;
using System.IO;
using MonoTouch.Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(GraphyPCL.iOS.PhotoService))]
namespace GraphyPCL.iOS
{
    public class PhotoService : IPhotoService
    {
        public async void SaveImageToDisk(ImageSource imgSrc, string name)
        {
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fullFilePath = Path.Combine(documentsDirectory, name + ".jpg");

            var renderer = new StreamImagesourceHandler();
            var photo = await renderer.LoadImageAsync(imgSrc);
            var imgData = photo.AsJPEG();
            NSError err = null;

            // Need check if exist!!
            if (!imgData.Save(fullFilePath, false, out err))
            {
                throw new Exception(err.LocalizedDescription);
            }
        }

        public ImageSource LoadImageFromDisk(string fileName, string folder = null, string extension = "jpg")
        {
            folder = (folder) ?? Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fullFilePath = Path.Combine(folder, fileName + "." + extension);

            return ImageSource.FromFile(fullFilePath);
        }

        public PhotoService()
        {
        }
    }
}

