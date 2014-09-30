using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public interface IPhotoService
    {
        /// <summary>
        /// Saves the picture to disk. The default extension is jpg.
        /// </summary>
        /// <param name="imgSrc">Image source.</param>
        /// <param name="name">Name.</param>
        void SaveImageToDisk(ImageSource imgSrc, string name);

        /// <summary>
        /// Loads the image from disk.
        /// </summary>
        /// <returns>The image from disk.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="folder">Folder. Default to Personal</param>
        /// <param name="extension">Extension.</param>
        ImageSource LoadImageFromDisk(string fileName, string folder = null, string extension = "jpg");
    }
}