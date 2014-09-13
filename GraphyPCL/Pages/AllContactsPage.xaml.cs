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
            this.ToolbarItems.Add(new ToolbarItem("add", "plus_icon.png", () => {}));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel = new AllContactsViewModel();
            _contactsList.ItemsSource = _viewModel.ContactsGroupCollection;
        }

        protected void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contact = e.SelectedItem as Contact;
            var contactDetailsPage = new ContactDetailsPage(contact);
            this.Navigation.PushAsync(contactDetailsPage);
        }
    }
}

