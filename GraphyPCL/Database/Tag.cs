using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Tag : IIdContainer, INameContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        // Note: Tag name is actually the primary key. However, we use TagId as a surrogate primary key.
        public string Name { get; set; }
    }
}