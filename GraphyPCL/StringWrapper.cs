using System;

namespace GraphyPCL
{
    public class StringWrapper : NotifyPropertyChangedObject
    {
        private string _innerString;

        public string InnerString
        {
            get
            {
                return _innerString;
            }
            set
            {
                SetProperty(ref _innerString, value);
            }
        }

        public StringWrapper()
        {
        }

        public StringWrapper(string innerString)
        {
            InnerString = innerString;
        }
    }
}