using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class AllContactsPage : ContentPage
    {
        private ContactsViewModel _viewModel;

        public AllContactsPage()
        {
            InitializeComponent();
            this.ToolbarItems.Add(new ToolbarItem("Add", "plus_icon.png", OnAddButtonClick));

            var allContacts = DatabaseManager.GetRows<Contact>();
            _viewModel = new ContactsViewModel(allContacts);
            BindingContext = _viewModel;

            _contactList.GroupDisplayBinding = new Binding("Title");
            _contactList.GroupShortNameBinding = new Binding("Title");
            _contactList.IsGroupingEnabled = true;
        }

        protected virtual void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var contact = e.SelectedItem as Contact;
                var contactDetailsPage = new ContactDetailsPage(contact);
                this.Navigation.PushAsync(contactDetailsPage);
                _contactList.SelectedItem = null;
            }
        }

        private void OnAddButtonClick()
        {
            var addContactPage = new AddEditContactPage();
            Navigation.PushAsync(addContactPage);
        }
    }
}

