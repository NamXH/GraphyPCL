using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphyPCL
{
    public class AllContactsViewModel
    {
        public ObservableCollection<ContactsGroup> ContactsGroupCollection { get; set; }

        public AllContactsViewModel()
        {
            var allContacts = DatabaseManager.GetRows<Contact>();
            ContactsGroupCollection = CreateContactsGroupCollection(allContacts);
        }

        private ObservableCollection<ContactsGroup> CreateContactsGroupCollection(IList<Contact> contacts)
        {
            var contactsGroupedByFirstChar = new Dictionary<string, ContactsGroup>();
            foreach (var contact in contacts)
            {
                var firstCharOfFullName = (String.IsNullOrEmpty(contact.FullName)) ? " " : contact.FullName.Substring(0, 1).ToUpperInvariant();

                ContactsGroup group;
                var firstCharNotInDictionary = !contactsGroupedByFirstChar.TryGetValue(firstCharOfFullName, out group);
                if (firstCharNotInDictionary)
                {
                    group = new ContactsGroup(firstCharOfFullName);
                    contactsGroupedByFirstChar.Add(firstCharOfFullName, group);
                }
                group.Contacts.Add(contact);
            }
            return new ObservableCollection<ContactsGroup>(contactsGroupedByFirstChar.Values.OrderBy(x => x.Title));
        }
    }
}

