using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Url : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public Guid ContactId { get; set; }
    }
}