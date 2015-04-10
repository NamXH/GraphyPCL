using System;

namespace GraphyPCL
{
    /// <summary>
    /// The detailed version of the tags which contains information from 3 tables Tag, ContactTagMap, and Contact.
    /// (like a JOIN query)
    /// </summary>
    public class CompleteTag : NotifyPropertyChangedObject
    {
        public Guid TagId { get; set; }

        public string Name { get; set; }

        private string _detail;

        public string Detail
        { 
            get
            {
                return _detail;
            }
            set
            {
                SetProperty(ref _detail, value);
            }
        }

        private string _newTagName;

        public string NewTagName
        { 
            get
            {
                return _newTagName;
            }
            set
            {
                SetProperty(ref _newTagName, value);
            }
        }
    }
}