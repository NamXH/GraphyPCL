using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Url : IIdContainer, IContactIdRelated, ITypeValuePairContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        private string _link;

        public string Link
        { 
            get
            {
                return _link;
            }
            set
            {
                _link = value;
            }
        }

        [Ignore]
        public string Value
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
            }
        }

        public Guid ContactId { get; set; }
    }
}