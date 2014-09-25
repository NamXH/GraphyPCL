﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class ContactDetailsViewModel
    {
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

        public ContactDetailsViewModel()
        {
            Contact = new Contact();
        }

        public ContactDetailsViewModel(Contact contact)
        {
            this.Contact = contact;
            Organization = contact.Organization ?? " ";

            PhoneNumbers = DatabaseManager.GetRowsRelatedToContact<PhoneNumber>(contact.Id);
            Emails = DatabaseManager.GetRowsRelatedToContact<Email>(contact.Id);
            Urls = DatabaseManager.GetRowsRelatedToContact<Url>(contact.Id);
            Addresses = DatabaseManager.GetRowsRelatedToContact<Address>(contact.Id);

            var birthdayIsNotDefault = !DateTime.Equals(contact.Birthday, new DateTime(1, 1, 1));
            if (birthdayIsNotDefault)
            {
                this.BirthdayShortForm = contact.Birthday.ToString("d");
            }
            else
            {
                this.BirthdayShortForm = "";
            }

            SpecialDates = DatabaseManager.GetRowsRelatedToContact<SpecialDate>(contact.Id);
            IMs = DatabaseManager.GetRowsRelatedToContact<InstantMessage>(contact.Id);

            var tagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(contact.Id);
            var tagIds = new List<int>();
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
    }
}