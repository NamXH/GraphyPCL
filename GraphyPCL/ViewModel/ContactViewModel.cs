using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GraphyPCL
{
    // May have to split this view model
    public class ContactViewModel
    {
        private const string c_datetimeFormat = "MMM dd yyyy";

        public Contact Contact { get; set; }

        public string Organization { get; set; }

        public IList<PhoneNumber> PhoneNumbers { get; set; }

        public IList<Email> Emails { get; set; }

        public IList<Url> Urls { get; set; }

        public IList<Address> Addresses { get; set; }

        public IList<SpecialDate> SpecialDates { get; set; }

        public IList<InstantMessage> IMs { get; set; }

        public IList<Tag> Tags { get; set; }

        public IList<TagAndDetail> TagsAndDetails { get; set; }

        public IList<ContactTagMap> ContactTagMaps { get; set; }

        public IList<RelatedContact> ContactsLinkedFromThisContact { get; set; }

        public IList<RelatedContact> ContactsLinkedToThisContact { get; set; }

        public IList<CompleteRelationship> CompleteRelationships { get; set; }

        public IList<RelationshipType> RelationshipTypes { get; set; }

        private ICommand _selectContactPhotoCommand;

        public ICommand SelectContactPhotoCommand
        {
            get
            {
                return _selectContactPhotoCommand;
            }
        }

        /// <summary>
        /// Use when creating new Contact
        /// </summary>
        public ContactViewModel()
        {
            Contact = new Contact();
            _selectContactPhotoCommand = new Command(SelectContactPhoto);

            Tags = DatabaseManager.GetRows<Tag>();
            RelationshipTypes = DatabaseManager.GetRows<RelationshipType>();

            CompleteRelationships = new List<CompleteRelationship>();
        }

        /// <summary>
        /// Use when editting an existing Contact
        /// </summary>
        public ContactViewModel(Contact contact)
        {
            this.Contact = contact;
            Organization = contact.Organization ?? " ";

            PhoneNumbers = DatabaseManager.GetRowsRelatedToContact<PhoneNumber>(contact.Id);
            Emails = DatabaseManager.GetRowsRelatedToContact<Email>(contact.Id);
            Urls = DatabaseManager.GetRowsRelatedToContact<Url>(contact.Id);
            Addresses = DatabaseManager.GetRowsRelatedToContact<Address>(contact.Id);

            SpecialDates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(contact.Id);
            IMs = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(contact.Id);

            ContactTagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(contact.Id);
            TagsAndDetails = new List<TagAndDetail>();
            foreach (var tagMap in ContactTagMaps)
            {
                var tag = DatabaseManager.GetRow<Tag>(tagMap.TagId);
                TagsAndDetails.Add(new TagAndDetail
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Detail = tagMap.Detail
                    });
            }

            // Relationship (From)
            var fromRelationships = DatabaseManager.GetRelationshipsFromContact(contact.Id);
            ContactsLinkedFromThisContact = new List<RelatedContact>();
            foreach (var relationship in fromRelationships)
            {
                var type = DatabaseManager.GetRow<RelationshipType>(relationship.RelationshipTypeId).Name;
                var relatedContact = DatabaseManager.GetRow<Contact>(relationship.ToContactId);
                var relationshipDetail = relationship.Detail;
                ContactsLinkedFromThisContact.Add(new RelatedContact(relatedContact, type, relationshipDetail));
            }

            // Relationship (To)
            // A bit duplicate, refactor later!!
            var toRelationships = DatabaseManager.GetRelationshipsToContact(contact.Id);
            ContactsLinkedToThisContact = new List<RelatedContact>();
            foreach (var relationship in toRelationships)
            {
                var type = DatabaseManager.GetRow<RelationshipType>(relationship.RelationshipTypeId).Name;
                var relatedContact = DatabaseManager.GetRow<Contact>(relationship.FromContactId);
                var relationshipDetail = relationship.Detail;
                ContactsLinkedToThisContact.Add(new RelatedContact(relatedContact, type, relationshipDetail));
            }
        }

        private async void SelectContactPhoto()
        {
            var mediaPicker = DependencyService.Get<IMediaPicker>();
            MediaFile mediaFile = null;
            try
            {
                mediaFile = await mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                    {
                        MaxPixelDimension = 1024
                    });
            }
            catch (TaskCanceledException)
            {
                // If TaskCanceledException is thrown: user cancel then do nothing!!
            }

            var randomName = Guid.NewGuid().ToString() + ".jpg";
            var imageSource = ImageSource.FromStream(() => mediaFile.Source);
            await DependencyService.Get<IPhotoService>().SaveImageToDisk(imageSource, randomName);

            Contact.ImageName = randomName;
        }

        /// <summary>
        /// Saves the new contact and all information in this View Model to database.
        /// </summary>
        public void SaveNewContact()
        {
            var db = DatabaseManager.DbConnection;

            // Insert new contact
            Contact.Id = Guid.NewGuid();
            db.Insert(Contact);

            // Insert basic info of new contact
            DatabaseManager.InsertList(PhoneNumbers, Contact);
            DatabaseManager.InsertList(Emails, Contact);
            DatabaseManager.InsertList(Urls, Contact);
            DatabaseManager.InsertList(Addresses, Contact);
            DatabaseManager.InsertList(SpecialDates, Contact);
            DatabaseManager.InsertList(IMs, Contact);

            // Insert related info to new contact: tags, relationships
            var existingTags = DatabaseManager.GetRows<Tag>();
            foreach (var tag in this.Tags)
            {
                var notAlreadyExists = existingTags.Where(x => x.Id == tag.Id).Count() == 0;
                if (notAlreadyExists)
                {
                    DatabaseManager.DbConnection.Insert(tag);
                }
            }
            DatabaseManager.InsertList(ContactTagMaps, Contact);

            foreach (var relationship in CompleteRelationships)
            {
                var emptyRelationshipType = (relationship.RelationshipTypeId == null) && String.IsNullOrEmpty(relationship.NewRelationshipName);
                if (String.IsNullOrEmpty(relationship.RelatedContactName) || emptyRelationshipType)
                {
                    continue;
                }

                var newRelationship = new Relationship();
                newRelationship.Detail = relationship.Detail;
                newRelationship.Id = Guid.NewGuid();

                if (!String.IsNullOrEmpty(relationship.NewRelationshipName))
                {
                    var newRelationshipType = new RelationshipType();
                    newRelationshipType.Id = Guid.NewGuid();
                    newRelationshipType.Name = relationship.NewRelationshipName;
                    DatabaseManager.DbConnection.Insert(newRelationshipType);
                    newRelationship.RelationshipTypeId = newRelationshipType.Id;
                }
                else
                {
                    newRelationship.RelationshipTypeId = relationship.RelationshipTypeId;
                }

                if (relationship.IsToRelatedContact)
                {
                    newRelationship.ToContactId = relationship.RelatedContactId;
                    newRelationship.FromContactId = Contact.Id;
                }
                else
                {
                    newRelationship.ToContactId = Contact.Id;
                    newRelationship.FromContactId = relationship.RelatedContactId;
                }

                DatabaseManager.DbConnection.Insert(newRelationship);
            }
        }
    }
}