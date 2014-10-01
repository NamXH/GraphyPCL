using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class InstantMessage : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Nickname { get; set; }

        public int ContactId { get; set; }
    }
}