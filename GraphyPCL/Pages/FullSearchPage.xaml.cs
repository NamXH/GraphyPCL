using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class FullSearchPage : ContentPage
    {
        public IList<StringWrapper> Criteria { get; set; }

        public FullSearchPage()
        {
            InitializeComponent();
            _tableView.Intent = TableIntent.Menu;

            Criteria = new List<StringWrapper>();
            new AddMoreEntryCell(_tableView, _criteriaSection, Criteria);

            _searchButton.Clicked += (object sender, EventArgs e) =>
            {
                var contacts = SearchContacts();
                var resultPage = new ContactsPage(contacts, false);
                resultPage.Title = "Search Result";
                Navigation.PushAsync(resultPage);
            };
        }

        public IList<Contact> SearchContacts()
        {
            IList<Contact> eligibleContacts = new List<Contact>();

            var contacts = DatabaseManager.GetRows<Contact>();

            foreach (var criterion in Criteria)
            {
                if (String.IsNullOrEmpty(criterion.InnerString))
                {
                    continue;
                }

//                var basicFilteredContacts = 

            }


            return eligibleContacts;
        }

        /// <summary>
        /// Filters contact by basic info: first name, middle name, last name, organization.
        /// </summary>
        /// <returns>The by basic info.</returns>
        /// <param name="criterion">Criterion.</param>
        /// <param name="contacts">Contacts.</param>
        public IList<Contact> FilterByBasicInfo(string criterion, IList<Contact> contacts)
        {
            var filteredContacts = new List<Contact>();

            if (!String.IsNullOrEmpty(criterion))
            {
                foreach (var contact in contacts)
                {
                    if (String.Equals(contact.FirstName, criterion, StringComparison.OrdinalIgnoreCase) 
                        || String.Equals(contact.MiddleName, criterion, StringComparison.OrdinalIgnoreCase)
                        || String.Equals(contact.LastName, criterion, StringComparison.OrdinalIgnoreCase)
                        || String.Equals(contact.Organization, criterion, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredContacts.Add(contact);
                    }
                }
            }

            return filteredContacts;
        }

        /// <summary>
        /// Filters contacts by the tags they contain.
        /// </summary>
        /// <returns>The by tag.</returns>
        /// <param name="criterion">Criterion.</param>
        /// <param name="contacts">Contacts.</param>
        public IList<Contact> FilterByTag(string criterion, IList<Contact> contacts)
        {
            var filteredContacts = new List<Contact>();

            if (!String.IsNullOrEmpty(criterion))
            {
                var tag = DatabaseManager.GetRowsByNameIgnoreCaseFirstLetter<Tag>(criterion).SingleOrDefault();

                if (tag != null)
                {
                    var contactTagMaps = DatabaseManager.DbConnection.Table<ContactTagMap>().Where(x => x.TagId == tag.Id).ToList();
                    var relatedContacts = new List<Contact>();

                    foreach (var contactTagMap in contactTagMaps)
                    {
                        var relatedContact = DatabaseManager.DbConnection.Table<Contact>().FirstOrDefault(x => x.Id == contactTagMap.ContactId);
                        if (relatedContact != null)
                        {
                            relatedContacts.Add(relatedContact);
                        }
                    }

                    filteredContacts = contacts.Intersect(relatedContacts, new ContactComparer()).ToList();
                }
            }

            return filteredContacts;
        }

        /// <summary>
        /// Filters contacts by the relationships they contain.
        /// </summary>
        /// <returns>The by relationship.</returns>
        /// <param name="criterion">Criterion.</param>
        /// <param name="contacts">Contacts.</param>
        public IList<Contact> FilterByRelationship(string criterion, IList<Contact> contacts)
        {
            var filteredContacts = new List<Contact>();

            if (!String.IsNullOrEmpty(criterion))
            {
                var relationshipType = DatabaseManager.GetRowsByNameIgnoreCaseFirstLetter<RelationshipType>(criterion).SingleOrDefault();
                if (relationshipType != null)
                {
                    var relationships = DatabaseManager.DbConnection.Table<Relationship>().Where(x => x.RelationshipTypeId == relationshipType.Id);
                    var relatedContacts = new List<Contact>();

                    foreach (var relationship in relationships)
                    {
                        relatedContacts.Add(DatabaseManager.GetRow<Contact>(relationship.FromContactId));
                        relatedContacts.Add(DatabaseManager.GetRow<Contact>(relationship.ToContactId));
                    }

                    filteredContacts = contacts.Intersect(relatedContacts, new ContactComparer()).ToList();
                }
            }

            return filteredContacts;
        }
    }
}