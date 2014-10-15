using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class AllContactsPage : ContentPage
    {
        private AllContactsViewModel _viewModel;

        public AllContactsPage()
        {
            InitializeComponent();
            this.ToolbarItems.Add(new ToolbarItem("Add", "plus_icon.png", OnAddButtonClick));

            _viewModel = new AllContactsViewModel();
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
            var addContactPage = new AddContactPage();
            Navigation.PushAsync(addContactPage);
        }
    }
}

