using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class SpecialDate : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public Guid ContactId { get; set; }
    }
}