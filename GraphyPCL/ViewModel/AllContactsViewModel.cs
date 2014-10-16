using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AllContactsViewModel
    {
        public ObservableCollection<ContactsGroup> ContactsGroupCollection { get; set; }

        public AllContactsViewModel()
        {
            var allContacts = DatabaseManager.GetRows<Contact>();
            ContactsGroupCollection = CreateContactsGroupCollection(allContacts);
            MessagingCenter.Subscribe<ContactDetailsPage, Contact>(this, "Delete", (sender, args) =>
                {
                    var contactToRemove = (Contact)args;
                    var contactGroupContainsContactToRemove = ContactsGroupCollection.Where(x => x.Title == contactToRemove.FullName[0].ToString().ToUpperInvariant()).FirstOrDefault();
                    if (contactGroupContainsContactToRemove == null)
                    {
                        throw new Exception(String.Format("Cannot find contact {0} with Id {1} in list of contacts", contactToRemove.FullName, contactToRemove.Id));
                    }
                    var success = contactGroupContainsContactToRemove.Contacts.Remove(contactToRemove);
                    if (!success)
                    {
                        throw new Exception(String.Format("Cannot find contact {0} with Id {1} in list of contacts", contactToRemove.FullName, contactToRemove.Id));

                    }
                });
        }

        private ObservableCollection<ContactsGroup> CreateContactsGroupCollection(IList<Contact> contacts)
        {
            var contactsGroupedByFirstChar = new Dictionary<string, ContactsGroup>();
            foreach (var contact in contacts)
            {
                var firstCharOfFullName = (String.IsNullOrEmpty(contact.FullName)) ? "#" : contact.FullName[0].ToString().ToUpperInvariant();
                if (!Char.IsLetter(firstCharOfFullName[0]))
                {
                    firstCharOfFullName = "#";
                }

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

