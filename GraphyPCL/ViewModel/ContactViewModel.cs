﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class ContactViewModel
    {
        private const string c_datetimeFormat = "MMM dd yyyy";
        private readonly DateTime _defaultSystemDateTime = new DateTime(1, 1, 1);
        private readonly DateTime _defaultPickerDateTime = new DateTime(1900, 1, 1);

        public Contact Contact { get; set; }

        public string Organization { get; set; }

        public IList<PhoneNumber> PhoneNumbers { get; set; }

        public IList<Email> Emails { get; set; }

        public IList<Url> Urls { get; set; }

        public IList<Address> Addresses { get; set; }

        public string BirthdayShortForm { get; set; }

        public IList<SpecialDate> SpecialDates { get; set; }

        public IList<InstantMessage> IMs { get; set; }

        public IList<Tag> Tags { get; set; }

        public IList<RelatedContact> ContactsLinkedFromThisContact { get; set; }

        public IList<RelatedContact> ContactsLinkedToThisContact { get; set; }

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

            var birthdayIsNotDefault = !DateTime.Equals(contact.Birthday, _defaultSystemDateTime);
            if (birthdayIsNotDefault)
            {
                this.BirthdayShortForm = contact.Birthday.ToString(c_datetimeFormat);
            }
            else
            {
                this.BirthdayShortForm = "";
            }

            SpecialDates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(contact.Id);
            IMs = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(contact.Id);

            var tagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(contact.Id);
            var tagIds = new List<Guid>();
            foreach (var tagMap in tagMaps)
            {
                tagIds.Add(tagMap.TagId);
            }
            Tags = DatabaseManager.GetRows<Tag>(tagIds);

            // Relationship (From)
            var fromRelationships = DatabaseManager.GetRelationshipsFromContact(contact.Id);
            ContactsLinkedFromThisContact = new List<RelatedContact>();
            foreach (var relationship in fromRelationships)
            {
                var type = DatabaseManager.GetRow<RelationshipType>(relationship.RelationshipTypeId).Name;
                var relatedContact = DatabaseManager.GetRow<Contact>(relationship.ToContactId);
                var relationshipDetail = relationship.ExtraInfo;
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
                var relationshipDetail = relationship.ExtraInfo;
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

        public void SaveNewContact()
        {
            var db = DatabaseManager.DbConnection;

            Contact.Id = Guid.NewGuid();
            db.Insert(Contact);

            DatabaseManager.InsertList(PhoneNumbers, Contact);
            DatabaseManager.InsertList(Emails, Contact);
            DatabaseManager.InsertList(Urls, Contact);
            DatabaseManager.InsertList(Addresses, Contact);
            DatabaseManager.InsertList(SpecialDates, Contact);
            DatabaseManager.InsertList(IMs, Contact);
        }
    }
}