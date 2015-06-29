﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphyPCL
{
    public static class UserDataManager
    {
        public static UserData UserData { get; set; }

        public static void CalculateAndSaveNormalFieldCount(Contact contact, IList<PhoneNumber> phoneNumbers, IList<Email> emails, IList<Url> urls, IList<InstantMessage> ims, IList<Address> addresses, IList<SpecialDate> specialDates)
        {
            var count = 0;

            if (!String.IsNullOrWhiteSpace(contact.FirstName))
            {
                count++;
            }
            if (!String.IsNullOrWhiteSpace(contact.MiddleName))
            {
                count++;
            }
            if (!String.IsNullOrWhiteSpace(contact.LastName))
            {
                count++;
            }
            if (!String.IsNullOrWhiteSpace(contact.Organization))
            {
                count++;
            }

            // Don't check if a field is empty string or white spaces. Remember to tell test users to not include empty fields.
            contact.NormalFieldCount = count 
                + phoneNumbers.Count 
                + emails.Count
                + urls.Count
                + ims.Count
                + addresses.Count
                + specialDates.Count;
        }

        public static void CalculateAndSaveTagCount(Contact contact, IList<CompleteTag> completeTags)
        {
            var autoAddedTagCount = 0;
            
            foreach (var autoAddedTagName in DatabaseManager.AutoAddedTagNames)
            {
                if (completeTags.Any(x => (x.NewTagName == autoAddedTagName) || ((String.IsNullOrWhiteSpace(x.NewTagName) && (x.Name == autoAddedTagName)))))
                {
                    autoAddedTagCount++;
                }
            }

            contact.AutoAddedTagCount = autoAddedTagCount;
            contact.CustomTagCount = completeTags.Count - autoAddedTagCount;
        }

        public static void CalculateAndSaveRelationshipCount(Contact contact, IList<CompleteRelationship> completeRelationships)
        {
            contact.RelationshipCount = completeRelationships.Count;
        }

        public static void CalculateAndSaveUserMetrics(Contact contact, IList<PhoneNumber> phoneNumbers, IList<Email> emails, IList<Url> urls, IList<InstantMessage> ims, IList<Address> addresses, IList<SpecialDate> specialDates, IList<CompleteTag> completeTags, IList<CompleteRelationship> completeRelationships)
        {
            CalculateAndSaveNormalFieldCount(contact, phoneNumbers, emails, urls, ims, addresses, specialDates);
            CalculateAndSaveTagCount(contact, completeTags);
            CalculateAndSaveRelationshipCount(contact, completeRelationships);

            var allFieldCount = contact.NormalFieldCount + contact.CustomTagCount + contact.AutoAddedTagCount + contact.RelationshipCount;

            if (contact.CustomTagCount != 0)
            {
                contact.CustomTagWeight = (double)contact.CustomTagCount / allFieldCount;
                // else customTagWeight default = 0
            }

            if (contact.AutoAddedTagCount != 0)
            {
                contact.AutoAddedTagWeight = (double)contact.AutoAddedTagCount / allFieldCount;
            }

            if (contact.CustomTagCount + contact.AutoAddedTagCount != 0)
            {
                contact.TagWeight = (contact.CustomTagCount + contact.AutoAddedTagCount) / (double)allFieldCount;
            }

            if (contact.RelationshipCount != 0)
            {
                contact.RelationshipWeight = (double)contact.RelationshipCount / allFieldCount;
            }
        }
    }
}