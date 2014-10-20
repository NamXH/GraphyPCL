using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Email : IIdContainer, IContactIdRelated, ITypeValuePairContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        private string _address;

        public string Address
        { 
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public string Value
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public Guid ContactId { get; set; }
    }
}