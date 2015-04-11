using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class RelationshipType : IIdContainer, INameContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        // Note: Relationship name is actually the primary key. However, we use Id as a surrogate primary key.
        public string Name { get; set; }
    }
}