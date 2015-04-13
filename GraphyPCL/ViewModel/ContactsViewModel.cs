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
//            
            Contacts = contacts;
            ContactsGroupCollection = CreateContactsGroupCollection(Contacts);
            MessagingCenter.Subscribe<ContactDetailsPage, Contact>(this, "Delete", (sender, args) =>
                {
                    var contactToRemove = (Contact)args;
                    var contactGroupContainsContactToRemove = ContactsGroupCollection.Where(x => x.Title == contactToRemove.FirstCharOfFullName).FirstOrDefault();
                    if (contactGroupContainsContactToRemove == null)
                    {
                        throw new Exception(String.Format("Cannot find contact {0} with Id {1} in list of contacts", contactToRemove.FullName, contactToRemove.Id));
                    }

                    var success = contactGroupContainsContactToRemove.Remove(contactToRemove);
                    if (!success)
                    {
                        throw new Exception(String.Format("Cannot find contact {0} with Id {1} in list of contacts", contactToRemove.FullName, contactToRemove.Id));
                    }

                    if (contactGroupContainsContactToRemove.Count == 0)
                    {
                        ContactsGroupCollection.Remove(contactGroupContainsContactToRemove);
                    }
                });

            MessagingCenter.Subscribe<AddEditContactPage, Contact>(this, "Add", (sender, args) =>
                {
                    var contactToAdd = (Contact)args;
                    var firstChar = contactToAdd.FirstCharOfFullName;
                    var contactGroupContainsContactToAdd = ContactsGroupCollection.Where(x => x.Title == firstChar).FirstOrDefault();
                    if (contactGroupContainsContactToAdd == null)
                    {
                        var newContactGroup = new ContactsGroup(firstChar);
                        newContactGroup.Add(contactToAdd);

                        if (ContactsGroupCollection.Count == 0)
                        {
                            ContactsGroupCollection.Add(newContactGroup);
                        }
                        else
                        {
                            var index = 0;
                            while (String.Compare(ContactsGroupCollection[index].Title, firstChar) < 0)
                            {
                                index++;
                            }
                            ContactsGroupCollection.Insert(index - 1, newContactGroup);
                        }
                    }
                    else
                    {
                        contactGroupContainsContactToAdd.Add(contactToAdd);
                    }
                });

            MessagingCenter.Subscribe<AddEditContactPage, Contact>(this, "Update", (sender, args) =>
                {
                    UpdateContact((Contact)args);
                });
            
            // Workaround for a bug!!
            // On EditContactPage, if user pushes Back button, everything will be save instead of ignored
            MessagingCenter.Subscribe<AllContactsNavigationPage, Contact>(this, "Update", (sender, args) =>
                {
                    UpdateContact((Contact)args);
                });
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

        private void UpdateContact(Contact contactToUpdate)
        {
            var contactGroupContainsContactToUpdate = ContactsGroupCollection.Where(x => x.Title == contactToUpdate.FirstCharOfFullName).FirstOrDefault();
            if (contactGroupContainsContactToUpdate == null)
            {
                throw new Exception(String.Format("Cannot find contact {0} with Id {1} in list of contacts", contactToUpdate.FullName, contactToUpdate.Id));
            }

            // Remove then add back
            var success = contactGroupContainsContactToUpdate.Remove(contactToUpdate);
            if (!success)
            {
                throw new Exception(String.Format("Cannot find contact {0} with Id {1} in list of contacts", contactToUpdate.FullName, contactToUpdate.Id));
            }
            contactGroupContainsContactToUpdate.Add(contactToUpdate); 
        }
    }
}