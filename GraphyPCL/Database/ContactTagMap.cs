using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class ContactTagMap : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }

        public Guid TagId { get; set; }

        public string Detail { get; set; }
    }
}