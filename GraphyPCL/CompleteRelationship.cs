using System;

namespace GraphyPCL
{
    // A convenient class to use in View Models
    public class CompleteRelationship : NotifyPropertyChangedObject
    {
        private Guid _relationshipId;

        public Guid RelationshipId
        { 
            get
            {
                return _relationshipId;
            }
            set
            {
                SetProperty(ref _relationshipId, value);
            }
        }

        private string _detail;

        public string Detail
        { 
            get
            {
                return _detail;
            }
            set
            {
                SetProperty(ref _detail, value);
            }
        }

        private Guid _relatedContactId;

        public Guid RelatedContactId
        { 
            get
            {
                return _relatedContactId;
            }
            set
            {
                SetProperty(ref _relatedContactId, value);
            }
        }

        private string _relatedContactName;

        public string RelatedContactName
        { 
            get
            {
                return _relatedContactName;
            }
            set
            {
                SetProperty(ref _relatedContactName, value);
            }
        }

        private bool _isToRelatedContact;

        // If true: "=>", false: "<="
        public bool IsToRelatedContact
        { 
            get
            {
                return _isToRelatedContact;
            }
            set
            {
                SetProperty(ref _isToRelatedContact, value);
            }
        }

        private Guid _relationshipTypeId;

        public Guid RelationshipTypeId
        { 
            get
            {
                return _relationshipTypeId;
            }
            set
            {
                SetProperty(ref _relationshipTypeId, value);
            }
        }

        private string _relationshipTypeName;

        public string RelationshipTypeName
        { 
            get
            {
                return _relationshipTypeName;
            }
            set
            {
                SetProperty(ref _relationshipTypeName, value);
            }
        }

        private string _newRelationshipName;

        public string NewRelationshipName
        { 
            get
            {
                return _newRelationshipName;
            }
            set
            {
                SetProperty(ref _newRelationshipName, value);
            }
        }

        public CompleteRelationship()
        {
            IsToRelatedContact = true;
        }
    }
}