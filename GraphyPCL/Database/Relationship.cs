using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Relationship : IIdContainer
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string ExtraInfo { get; set; }

        public int FromContactId { get; set; }

        public int ToContactId { get; set; }

        public int RelationshipTypeId { get; set; }
    }
}