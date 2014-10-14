using System;

namespace GraphyPCL
{
    // A convenient class to use in View Models
    public class CompleteRelationship
    {
        public Guid RelationshipId { get; set; }

        public string Detail { get; set; }

        public Guid RelatedContactId { get; set; }

        public string RelatedContactName { get; set; }

        // If true: "=>", false: "<="
        public bool IsToRelatedContact { get; set; }

        public Guid RelationshipTypeId { get; set; }

        public string RelationshipTypeName { get; set; }

        public CompleteRelationship()
        {
            IsToRelatedContact = true;
        }
    }
}