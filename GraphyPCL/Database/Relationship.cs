using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Relationship : IIdContainer 
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string ExtraInfo { get; set; }

        public Guid FromContactId { get; set; }

        public Guid ToContactId { get; set; }

        public Guid RelationshipTypeId { get; set; }
    }
}