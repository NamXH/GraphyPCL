using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class PhoneNumber : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Number { get; set; }

        public int ContactId { get; set; }
    }
}