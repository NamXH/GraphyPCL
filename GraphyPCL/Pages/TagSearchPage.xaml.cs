using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class TagSearchPage : ContentPage
    {
        public IList<StringWrapper> Criteria { get; set; }

        public TagSearchPage()
        {
            InitializeComponent();
            _tableView.Intent = TableIntent.Menu;

            Criteria = new List<StringWrapper>();
            new AddMoreEntryCell(_tableView, _criteriaSection, Criteria);

            _searchButton.Clicked += (object sender, EventArgs e) =>
            {
                var contacts = SearchContacts();
                var resultPage = new ContactsPage(contacts);
                resultPage.Title = "Search Result";
                Navigation.PushAsync(resultPage);
            };
        }

        private IList<Contact> SearchContacts()
        {
            IList<Contact> eligibleContacts = new List<Contact>();

            var criteriaTags = new List<Tag>();
            foreach (var criterion in Criteria)
            {
                if (String.IsNullOrEmpty(criterion.InnerString))
                {
                    continue;
                }

                var tag = DatabaseManager.GetRowsByName<Tag>(criterion.InnerString).SingleOrDefault();
                if (tag == null)
                {
                    return new List<Contact>(); 
                }
                else
                {
                    criteriaTags.Add(tag);
                }
            }

            var allContacts = DatabaseManager.GetRows<Contact>();
            foreach (var contact in allContacts)
            {
                var eligible = true;
                var contactTagMaps = DatabaseManager.GetRowsRelatedToContact<ContactTagMap>(contact.Id);

                foreach (var tag in criteriaTags)
                {
                    var criteriaContactTagMap = contactTagMaps.FirstOrDefault(x => x.TagId.Equals(tag.Id));
                    if (criteriaContactTagMap == null) // Make sure contactTagMaps always contains at least one
                    {
                        eligible = false;
                        break;
                    }
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