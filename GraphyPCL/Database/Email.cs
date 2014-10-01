using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Email : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public int ContactId { get; set; }
    }
}