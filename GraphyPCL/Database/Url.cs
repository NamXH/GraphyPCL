using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Url : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public int ContactId { get; set; }
    }
}