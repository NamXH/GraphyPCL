using System;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class Contact : NotifyPropertyChangedObject//, IIdContainer
    {
        private const string c_defaultImageName = "unknown.jpg";

        [PrimaryKey]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Organization { get; set; }

        public DateTime Birthday { get; set; }

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

    }
}