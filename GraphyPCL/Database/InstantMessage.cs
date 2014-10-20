using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class InstantMessage : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Nickname { get; set; }

        public Guid ContactId { get; set; }
    }
}