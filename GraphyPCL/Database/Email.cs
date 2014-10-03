using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Email : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public Guid ContactId { get; set; }
    }
}