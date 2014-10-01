using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class ContactTagMap : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public int ContactId { get; set; }

        public int TagId { get; set; }
    }
}