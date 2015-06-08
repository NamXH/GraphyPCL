using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    // Should be refactored to be more MVVM / MVC !!
    public partial class FullSearchPage : ContentPage
    {
        public IList<StringWrapper> Criteria { get; set; }

        public FullSearchPage()
        {
            InitializeComponent();
            _tableView.Intent = TableIntent.Menu;

            Criteria = new List<StringWrapper>();
            new AddMoreEntryCell(_tableView, _criteriaSection, Criteria, "Keyword");

            _searchButton.Clicked += (object sender, EventArgs e) =>
            {
                var contacts = SearchContacts();
                var resultPage = new ContactsPage(contacts, false, 2);
                resultPage.Title = "Search Result";
                Navigation.PushAsync(resultPage);
            };
        }

        public IList<Contact> SearchContacts()
        {
            // Maybe IList or IEnum is enough here, we could improve performance !!
            var remainingContacts = DatabaseManager.GetRows<Contact>().ToList(); // start with all contacts

            // A contact is eligible if:
            // With each criterion, the contact has a basic info OR a tag OR a relationship type Equals to the criterion
            // (The contact has to satisfy all criterion)
            foreach (var criterion in Criteria)
            {
                if (String.IsNullOrEmpty(criterion.InnerString))
                {
                    continue;
                }

                var basicInfoEligibleContacts = FilterByBasicInfo(criterion.InnerString, remainingContacts);
                remainingContacts = remainingContacts.Except(basicInfoEligibleContacts, new ContactComparer()).ToList(); // Don't check already eligible contacts

                var tagEligibleContacts = FilterByTag(criterion.InnerString, remainingContacts);
                remainingContacts = remainingContacts.Except(tagEligibleContacts, new ContactComparer()).ToList(); // Don't check already eligible contacts

                var relationshipEligibleContacts = FilterByRelationship(criterion.InnerString, remainingContacts);

                // Remaining contacts is now assigned to all eligible contacts regarding this criterion (and previous criteria). Next criterion is only evaluated in the remainging contacts.
                remainingContacts = new List<Contact>();
                remainingContacts.AddRange(basicInfoEligibleContacts);
                remainingContacts.AddRange(tagEligibleContacts);
                remainingContacts.AddRange(relationshipEligibleContacts);
            }

//            // Evaluate result here (put in a function) !!
//            foreach (var contact in remainingContacts)
//            {
//                // Get all tags of the contact. Compare the tags' names with the criteria. Every match counts toward Tag effectiveness.
//                // Get all relationships of the contact. Compare the relationships' names with the criteria. Every match counts toward Tag effectiveness.
//            }

            return remainingContacts;
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
                    // this if is not robust
//                    if (String.Equals(contact.FirstName, criterion, StringComparison.OrdinalIgnoreCase)
//                        || String.Equals(contact.MiddleName, criterion, StringComparison.OrdinalIgnoreCase)
//                        || String.Equals(contact.LastName, criterion, StringComparison.OrdinalIgnoreCase)
//                        || String.Equals(contact.Organization, criterion, StringComparison.OrdinalIgnoreCase))

                    var lowerCaseCriterion = criterion.ToLower();

                    if ((!String.IsNullOrEmpty(contact.FirstName) && (contact.FirstName.ToLower().Contains(lowerCaseCriterion)))
                        || (!String.IsNullOrEmpty(contact.MiddleName) && (contact.MiddleName.ToLower().Contains(lowerCaseCriterion)))
                        || (!String.IsNullOrEmpty(contact.LastName) && (contact.LastName.ToLower().Contains(lowerCaseCriterion)))
                        || (!String.IsNullOrEmpty(contact.Organization) && (contact.Organization.ToLower().Contains(lowerCaseCriterion))))
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
                var tags = DatabaseManager.GetRowsContainNameIgnoreCase<Tag>(criterion);

                if ((tags != null) && (tags.Any()))
                {
                    var relatedContacts = new List<Contact>();

                    foreach (var tag in tags)
                    {
                        var contactTagMaps = DatabaseManager.DbConnection.Table<ContactTagMap>().Where(x => x.TagId == tag.Id).ToList();

                        foreach (var contactTagMap in contactTagMaps)
                        {
                            var relatedContact = DatabaseManager.DbConnection.Table<Contact>().FirstOrDefault(x => x.Id == contactTagMap.ContactId);
                            if (relatedContact != null)
                            {
                                relatedContacts.Add(relatedContact);
                            }
                        }
                    }

                    // Note: Intersect removes all duplicate
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
                var relationshipTypes = DatabaseManager.GetRowsContainNameIgnoreCase<RelationshipType>(criterion);

                if ((relationshipTypes != null) && (relationshipTypes.Any()))
                {
                    var relatedContacts = new List<Contact>();

                    foreach (var relationshipType in relationshipTypes)
                    {
                        var relationships = DatabaseManager.DbConnection.Table<Relationship>().Where(x => x.RelationshipTypeId == relationshipType.Id);

                        foreach (var relationship in relationships)
                        {
                            relatedContacts.Add(DatabaseManager.GetRow<Contact>(relationship.FromContactId));
                            relatedContacts.Add(DatabaseManager.GetRow<Contact>(relationship.ToContactId));
                        }
                    }

                    filteredContacts = contacts.Intersect(relatedContacts, new ContactComparer()).ToList();
                }
            }

            return filteredContacts;
        }
    }
}