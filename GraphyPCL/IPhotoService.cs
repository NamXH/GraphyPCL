using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GraphyPCL
{
    public interface IPhotoService
    {
        /// <summary>
        /// Saves the image to disk.
        /// </summary>
        /// <returns><c>true</c>, if image to disk was saved, <c>false</c> otherwise (if already exists same file name).</returns>
        /// <param name="imgSrc">Image source.</param>
        /// <param name="name">Name including extension if have.</param>
        Task<bool> SaveImageToDisk(ImageSource imgSrc, string name);

        /// <summary>
        /// Loads the image from disk.
        /// </summary>
        /// <returns>The image from disk.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="folder">Folder. Default to Personal folder</param>
        ImageSource LoadImageFromDisk(string name, string folder = null);
    }
}