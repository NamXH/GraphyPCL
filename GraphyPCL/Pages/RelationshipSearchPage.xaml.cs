using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class RelationshipSearchPage : ContentPage
    {
        public IList<CompleteRelationship> Criteria { get; set; }

        public RelationshipSearchPage()
        {
            InitializeComponent();
            _tableView.Intent = TableIntent.Menu;

            Criteria = new List<CompleteRelationship>();
            new AddMoreRelationshipCriteriaCell(_tableView, _criteriaSection, Criteria);

            _searchButton.Clicked += (object sender, EventArgs e) =>
            {
                var contacts = SearchContacts();
                Navigation.PushAsync(new FilteredContactsPage(contacts));
            };
        }

        private IList<Contact> SearchContacts()
        {
            IList<Contact> eligibleContacts = new List<Contact>();

            // Add RelationshipTypeId to Criteria. Now each criterion contains: RelationshipTypeName, RelationshipTypeId, IsToRelatedContact
            foreach (var criterion in Criteria)
            {
                if (String.IsNullOrEmpty(criterion.RelationshipTypeName))
                {
                    continue;
                }

                var type = DatabaseManager.GetRowsByName<RelationshipType>(criterion.RelationshipTypeName).SingleOrDefault();
                if (type == null)
                {
                    return new List<Contact>(); 
                }
                else
                {
                    criterion.RelationshipTypeId = type.Id;
                }
            }

            var allContacts = DatabaseManager.GetRows<Contact>();
            foreach (var contact in allContacts)
            {
                var eligible = true;

                var fromRelationships = DatabaseManager.GetRelationshipsFromContact(contact.Id);
                var toRelationships = DatabaseManager.GetRelationshipsToContact(contact.Id);

                foreach (var criterion in Criteria)
                {
                    if (String.IsNullOrEmpty(criterion.RelationshipTypeName))
                    {
                        continue;
                    }

                    Relationship relationship;
                    if (criterion.IsToRelatedContact)
                    {
                        relationship = fromRelationships.FirstOrDefault(x => x.RelationshipTypeId.Equals(criterion.RelationshipTypeId));
                    }
                    else
                    {
                        relationship = toRelationships.FirstOrDefault(x => x.RelationshipTypeId.Equals(criterion.RelationshipTypeId));
                    }
                    if (relationship == null) // Make sure contactTagMaps always contains at least one
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