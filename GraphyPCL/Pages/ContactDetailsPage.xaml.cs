using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class ContactDetailsPage : ContentPage
    {
        private const string c_datetimeFormat = "MMM dd yyyy";

        private ContactViewModel _viewModel;

        public ContactDetailsPage(Contact contact)
        {
            InitializeComponent();
            _tableView.Intent = TableIntent.Menu;
            this.ToolbarItems.Add(new ToolbarItem("Edit", null, () => {}));

            _viewModel = new ContactViewModel(contact);
            this.BindingContext = _viewModel;

            // List are not bindable to TableSection: PhoneNumbers, Emails, Urls
            // To do: create TableSection in Xaml then populate list inside each section to have flexible section order!!
            CreateUIList<PhoneNumber>(_tableRoot, _viewModel.PhoneNumbers, "PHONE", x => x.Number, x => x.Type);
            CreateUIList<Email>(_tableRoot, _viewModel.Emails, "EMAIL", x => x.Address, x => x.Type);
            CreateUIList<Url>(_tableRoot, _viewModel.Urls, "URL", x => x.Link, x => x.Type);

            // List are not bindable to TableSection: Addresses
            foreach (var address in _viewModel.Addresses)
            {
                var tableSection = new TableSection(address.Type + " ADDRESS");

                var line1IsNotNull = TryAddTextCell(tableSection, address.StreetLine1);
                var line2IsNotNull = TryAddTextCell(tableSection, address.StreetLine2);
                var cityAndProvince = String.IsNullOrEmpty(address.Province) ? address.City : address.City + ", " + address.Province;
                var cityAndProvinceIsNotNull = TryAddTextCell(tableSection, cityAndProvince);
                var countryIsNotNull = TryAddTextCell(tableSection, address.Country);
                var postalCodeIsNotNull = TryAddTextCell(tableSection, address.PostalCode);

                var sectionIsNotNull = line1IsNotNull || line2IsNotNull || cityAndProvinceIsNotNull
                                       || countryIsNotNull || postalCodeIsNotNull;
                if (sectionIsNotNull)
                {
                    _tableRoot.Add(tableSection);
                }
            }

            // List are not bindable to TableSection: SpecialDates, IMs, Tags
            CreateUIList<SpecialDate>(_tableRoot, _viewModel.SpecialDates, "SPECIAL DATES", x => x.Date.ToString(c_datetimeFormat), x => x.Type);
            CreateUIList<InstantMessage>(_tableRoot, _viewModel.IMs, "INSTANT MESSAGES", x => x.Nickname, x => x.Type);
            CreateUIList<TagAndDetail>(_tableRoot, _viewModel.TagsAndDetails, "TAGS", x => x.Name, x => x.Detail);

            // List are not bindable to TableSection: "From" and "To" Relationship
            CreateUIRelationshipList(_tableRoot, _viewModel.ContactsLinkedFromThisContact, "=>");
            CreateUIRelationshipList(_tableRoot, _viewModel.ContactsLinkedToThisContact, "<=");
        }

        /// <summary>
        /// Creates the user interface for a list.
        /// </summary>
        /// <param name="root">Root.</param>
        /// <param name="list">List.</param>
        /// <param name="sectionTitle">Section title.</param>
        /// <param name="CellText">Cell text.</param>
        /// <param name="CellDetail">Cell detail.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private void CreateUIList<T>(TableRoot root, IList<T> list, string sectionTitle, Func<T, string> CellText, Func<T, string> CellDetail)
        {
            if ((root == null) || (list == null) || (list.Count == 0))
            {
                return;
            }

            var tableSection = new TableSection(sectionTitle);
            root.Add(tableSection);

            foreach (var x in list)
            {
                var text = CellText(x);
                var textIsNotNullOrEmpty = !String.IsNullOrEmpty(text);
                if (textIsNotNullOrEmpty)
                {
                    var textCell = new TextCell();
                    textCell.Text = text;
                    textCell.Detail = CellDetail(x);
                    tableSection.Add(textCell);
                }
            }
        }

        private bool TryAddTextCell(TableSection container, string text)
        {
            var textIsNotNull = !String.IsNullOrEmpty(text);
            if (textIsNotNull)
            {
                var textCell = new TextCell();
                textCell.Text = text;
                container.Add(textCell);
            }
            return textIsNotNull;
        }

        private void CreateUIRelationshipList(TableRoot root, IList<RelatedContact> relatedContactList, string relationshipDirectionSymbol)
        {
            foreach (var relatedContact in relatedContactList)
            {
                var tableSection = new TableSection();
                tableSection.Title = relatedContact.RelationshipName ?? "";
                root.Add(tableSection);
                var relatedContactCell = new TextCell();
                tableSection.Add(relatedContactCell);
                relatedContactCell.Text = relationshipDirectionSymbol + " " + relatedContact.Contact.FullName;

                var detailIsNotNull = !String.IsNullOrEmpty(relatedContact.RelationshipDetail);
                if (detailIsNotNull)
                {
                    var detailCell = new ViewCell();
                    tableSection.Add(detailCell);
                    var layout = new StackLayout();
                    layout.Padding = new Thickness(15, 0, 0, 0); // Hard-coded padding. Need changed to a default value!!
                    detailCell.View = layout;
                    var label = new Label();
                    label.Text = relatedContact.RelationshipDetail;
                    layout.Children.Add(label);
                }
            }
        }
    }
}