using System;
using System.Collections.Generic;
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

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Contact other = obj as Contact;
            if ((System.Object)other == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Id == other.Id;
        }

        public bool Equals(Contact other)
        {
            // If parameter is null return false:
            if ((object)other == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class ContactComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact x, Contact y)
        {
            // Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            // Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }
            
            return x.Id == y.Id;
        }

        public int GetHashCode(Contact contact)
        {
            // Check whether the object is null. 
            if (Object.ReferenceEquals(contact, null))
            {
                return 0;
            }

            return contact.Id.GetHashCode();
        }
    }
}