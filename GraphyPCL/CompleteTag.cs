using System;

namespace GraphyPCL
{
    public class CompleteTag : NotifyPropertyChangedObject
    {
        public Guid Id { get; set; }

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