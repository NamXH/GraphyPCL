﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GraphyPCL
{
    // May have to split this view model into 2 for Add and Edit page
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

        public IList<CompleteTag> CompleteTags { get; set; }

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
            
        // May be refactor to use optional params but not required!!

        /// <summary>
        /// Use when creating new Contact
        /// </summary>
        public ContactViewModel()
        {
            Contact = new Contact();
            _selectContactPhotoCommand = new Command(SelectContactPhoto);

            PhoneNumbers = new List<PhoneNumber>();
            Emails = new List<Email>();
            Urls = new List<Url>();
            IMs = new List<InstantMessage>();

            Addresses = new List<Address>();
            SpecialDates = new List<SpecialDate>();

            Tags = DatabaseManager.GetRows<Tag>();
            CompleteTags = new List<CompleteTag>();

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
            IMs = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(contact.Id);

            Addresses = DatabaseManager.GetRowsRelatedToContact<Address>(contact.Id);
            SpecialDates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(contact.Id);

            Tags = DatabaseManager.GetRows<Tag>();
            ContactTagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(contact.Id);
            CompleteTags = new List<CompleteTag>();
            foreach (var tagMap in ContactTagMaps)
            {
                var tag = DatabaseManager.GetRow<Tag>(tagMap.TagId);
                CompleteTags.Add(new CompleteTag
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
            DatabaseManager.InsertList(IMs, Contact);
            DatabaseManager.InsertList(Addresses, Contact);
            DatabaseManager.InsertList(SpecialDates, Contact);

            // Insert related info to new contact: tags
            var existingTags = DatabaseManager.GetRows<Tag>();
            foreach (var completeTag in CompleteTags)
            {
                if (!String.IsNullOrEmpty(completeTag.NewTagName)) // Insert new created tag
                {
                    Guid tagId;
                    var tagsWithSameName = Tags.Where(x => x.Name == completeTag.NewTagName);
                    if (tagsWithSameName.Count() > 1)
                    {
                        throw new Exception("There are more than 1 new tags with the same name. We are not handling this!!"); // Serious!!
                    }
                    var existingTagWithSameName = existingTags.Where(x => x.Name == completeTag.NewTagName).FirstOrDefault();
                    if (existingTagWithSameName == null)
                    {
                        var newTag = new Tag
                        {
                            Id = Guid.NewGuid(),
                            Name = completeTag.NewTagName
                        };
                        DatabaseManager.DbConnection.Insert(newTag);
                        tagId = newTag.Id;
                    }
                    else
                    {
                        tagId = existingTagWithSameName.Id;
                    }
                    var newContactTagMap = new ContactTagMap
                    {
                        Id = Guid.NewGuid(),
                        Detail = completeTag.Detail,
                        ContactId = Contact.Id,
                        TagId = tagId
                    };
                    DatabaseManager.DbConnection.Insert(newContactTagMap);
                }
                else // Existing tag
                {
                    DatabaseManager.DbConnection.Insert(new ContactTagMap
                        {
                            Id = Guid.NewGuid(),
                            Detail = completeTag.Detail,
                            ContactId = Contact.Id,
                            TagId = completeTag.Id
                        });
                }
            }

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