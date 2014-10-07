using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Tag : IIdContainer, INameContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}