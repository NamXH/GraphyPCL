using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class Contact : NotifyPropertyChangedObject, IIdContainer
    {
        private const string c_defaultImageName = "unknown.jpg";

        [PrimaryKey]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public bool Favorite { get; set; }

        private string _imageName;

        public string ImageName
        { 
            get { return _imageName; }
            set
            {
                SetProperty(ref _imageName, value);
                OnPropertyChanged(() => this.Photo);
            }
        }

        #region additional properties that are not in DB table
        public string FullName
        {
            get
            {
                string firstName = !string.IsNullOrEmpty(FirstName) ? FirstName + " " : FirstName;
                string middleName = !string.IsNullOrEmpty(MiddleName) ? MiddleName + " " : MiddleName;
                string lastName = !string.IsNullOrEmpty(LastName) ? LastName : LastName;

                return firstName + middleName + lastName;
            }
        }

        public ImageSource Photo
        {
            get
            {
                var img = DependencyService.Get<IPhotoService>().LoadImageFromDisk(ImageName);
                img = img ?? ImageSource.FromFile(c_defaultImageName);

                return img;
            }
        }

        /// <summary>
        /// Use when grouping contact base on first char
        /// </summary>
        /// <value>Return "#" if FullName is null or empty, A string of the upper case of first char otherwise</value>
        public string FirstCharOfFullName
        {
            get
            {
                if (String.IsNullOrEmpty(FullName))
                {
                    return "#";
                }
                else
                {
                    var firstChar = FullName[0];
                    if (!Char.IsLetter(firstChar))
                    {
                        return "#";
                    }
                    else
                    {
                        return firstChar.ToString().ToUpperInvariant();
                    }
                }
            }
        }
        #endregion
    }
}