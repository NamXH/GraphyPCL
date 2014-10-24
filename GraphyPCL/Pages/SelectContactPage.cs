﻿using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    // Need refactor hierachy!!
    // Should exclude the contact initiates this page from the list
    public class SelectContactPage : AllContactsPage
    {
        public CompleteRelationship Relationship { get; set; }

        public SelectContactPage(CompleteRelationship relationship)
            : base()
        {
            this.ToolbarItems.Clear();
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