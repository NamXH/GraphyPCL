using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class RelationshipType : IIdContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}