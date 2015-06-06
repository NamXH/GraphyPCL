using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class BasicSearchPage : ContentPage
    {
        public BasicSearchPage()
        {
            InitializeComponent();

            _tableView.Intent = TableIntent.Menu;

            _searchButton.Clicked += (object sender, EventArgs e) =>
            {
                var contacts = SearchContacts();
                var resultPage = new ContactsPage(contacts, false);
                resultPage.Title = "Search Result";
                Navigation.PushAsync(resultPage);
            };
        }

        private IList<Contact> SearchContacts()
        {
            IList<Contact> eligibleContacts = new List<Contact>();

            var allContacts = DatabaseManager.GetRows<Contact>();
            foreach (var contact in allContacts)
            {
                var eligible = true;

                if (!String.IsNullOrEmpty(_firstNameEntry.Text) && (_firstNameEntry.Text != contact.FirstName))
                {
                    eligible = false;
                }

                if (!String.IsNullOrEmpty(_middleNameEntry.Text) && (_middleNameEntry.Text != contact.MiddleName))
                {
                    eligible = false;
                }

                if (!String.IsNullOrEmpty(_lastNameEntry.Text) && (_lastNameEntry.Text != contact.LastName))
                {
                    eligible = false;
                }

                if (!String.IsNullOrEmpty(_organizationEntry.Text) && (_organizationEntry.Text != contact.Organization))
                {
                    eligible = false;
                }

                if (eligible)
                {
                    eligibleContacts.Add(contact);
                }
            }

            return eligibleContacts;
        }
    }
}