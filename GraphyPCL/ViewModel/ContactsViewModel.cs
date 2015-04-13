using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class ContactsViewModel
    {
        public ObservableCollection<ContactsGroup> ContactsGroupCollection { get; set; }

        public IList<Contact> Contacts { get ; set; }

        public ContactsViewModel(IList<Contact> contacts)
        {
            Contacts = contacts;
            ContactsGroupCollection = CreateContactsGroupCollection(Contacts);
        }

        private ObservableCollection<ContactsGroup> CreateContactsGroupCollection(IList<Contact> contacts)
        {
            var contactsGroupedByFirstChar = new Dictionary<string, ContactsGroup>();
            foreach (var contact in contacts)
            {
                var firstCharOfFullName = contact.FirstCharOfFullName;

                ContactsGroup group;
                var firstCharNotInDictionary = !contactsGroupedByFirstChar.TryGetValue(firstCharOfFullName, out group);
                if (firstCharNotInDictionary)
                {
                    group = new ContactsGroup(firstCharOfFullName);
                    contactsGroupedByFirstChar.Add(firstCharOfFullName, group);
                }
                group.Add(contact);
            }
            return new ObservableCollection<ContactsGroup>(contactsGroupedByFirstChar.Values.OrderBy(x => x.Title));
        }
    }
}