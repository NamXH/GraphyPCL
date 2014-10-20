using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class InstantMessage : IIdContainer, IContactIdRelated, ITypeValuePairContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        private string _nickname;

        public string Nickname
        { 
            get
            {
                return _nickname;
            }
            set
            {
                _nickname = value;
            }
        }

        [Ignore]
        public string Value
        {
            get
            {
                return _nickname;
            }
            set
            {
                _nickname = value;
            }
        }

        public Guid ContactId { get; set; }
    }
}