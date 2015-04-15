using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    // Need refactor hierachy!!
    // Should exclude the contact initiates this page from the list
    public class SelectContactPage : ContactsPage
    {
        public CompleteRelationship Relationship { get; set; }

        public SelectContactPage(IList<Contact> contacts, CompleteRelationship relationship)
            : base(contacts, false)
        {
            this.Title = "Pick A Contact";
            Relationship = relationship;
        }

        protected override void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var contact = e.SelectedItem as Contact;
                Relationship.RelatedContactId = contact.Id;
                Relationship.RelatedContactName = contact.FullName;
                Navigation.PopAsync();
            }
        }
    }
}