using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AllContactsPage : ContactsPage
    {
        public AllContactsPage(IList<Contact> contacts)
            : base(contacts, true)
        {
            this.Title = "All Contacts";
            this.ToolbarItems.Add(new ToolbarItem("Add", "plus_icon.png", OnAddButtonClick));
        }

        protected override void SetViewModel()
        { 
            ViewModel = new AllContactsViewModel(Contacts);
            BindingContext = ViewModel;
        }

        private void OnAddButtonClick()
        {
            var addContactPage = new AddEditContactPage();
            Navigation.PushAsync(addContactPage);
        }
    }
}