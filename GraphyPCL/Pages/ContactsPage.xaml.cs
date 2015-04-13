using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    /// <summary>
    /// A page which display the list of contacts and provide access to each contact.
    /// </summary>
    public partial class ContactsPage : ContentPage
    {
        public ContactsViewModel ViewModel { get; set; }

        public IList<Contact> Contacts { get; set; }

        /// <summary>
        /// Create a page which display the list of contacts and provide access to each contact.
        /// </summary>
        /// <param name="contacts">Contacts.</param>
        public ContactsPage(IList<Contact> contacts)
        {
            InitializeComponent();

            Contacts = contacts;
            SetViewModel();

            _contactList.GroupDisplayBinding = new Binding("Title");
            _contactList.GroupShortNameBinding = new Binding("Title");
            _contactList.IsGroupingEnabled = true;
        }

        // Workaround: Any class inherit this class has to call base(contacts), it cannot implement its own constructor since it cannot access _contactList and other variables in the Xaml file !!
        protected virtual void SetViewModel()
        {
            ViewModel = new ContactsViewModel(Contacts);
            BindingContext = ViewModel; 
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
    }
}

