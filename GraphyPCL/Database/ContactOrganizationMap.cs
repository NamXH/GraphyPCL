using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class ContactOrganizationMap : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public Guid ContactId { get; set; }

        public Guid OrganizationId { get; set; }
    }
}