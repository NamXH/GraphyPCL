using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class PhoneNumber : IIdContainer, IContactIdRelated, ITypeValuePairContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        private string _number;

        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
            }
        }

        /// <summary>
        /// Reference to property Number. Only used for binding in a generic method.
        /// </summary>
        [Ignore]
        public string Value
        {
            get 
            {
                return _number;
            }
            set
            {
                _number = value;
            }
        }

        public Guid ContactId { get; set; }


    }
}