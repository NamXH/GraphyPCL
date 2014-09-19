using System;

namespace GraphyPCL
{
    public class RelatedContact
    {
        public Contact Contact { get; set; }

        public string RelationshipName { get; set; } 

        public string RelationshipDetail { get; set; }

        public RelatedContact(Contact contact, string relationshipName, string relationshipDetail = "")
        {
            Contact = contact;
            RelationshipName = relationshipName;
            RelationshipDetail = relationshipDetail;
        }
    }
}

