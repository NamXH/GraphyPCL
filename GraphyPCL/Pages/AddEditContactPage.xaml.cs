using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class AddEditContactPage : ContentPage
    {
        // These lists should go into view model!!
        private List<string> c_phoneTypes = new List<string>() { "mobile", "home", "work", "main", "other" };
        private List<string> c_emailTypes = new List<string>() { "home", "work", "main", "other" };
        private List<string> c_urlTypes = new List<string>() { "home", "work", "main", "other" };
        private List<string> c_imTypes = new List<string>() { "skype", "hangouts", "facebook", "msn", "yahoo", "aim", "qq", "other" };

        private ContactViewModel _viewModel;

        public ContactViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
        }

        public AddEditContactPage(Contact contact = null)
        {
            InitializeComponent();

            _tableView.Intent = TableIntent.Menu;
            this.ToolbarItems.Add(new ToolbarItem("Done", null, OnDoneButtonClicked));

            if (contact != null)
            {
                _viewModel = new ContactViewModel(contact); 
            }
            else
            {
                _viewModel = new ContactViewModel();
            }
            BindingContext = _viewModel;

            // Looks like a terrible design !!
            new AddMoreBasicElementCell<PhoneNumber>(_tableView, _phoneSection, c_phoneTypes, "Enter number", Keyboard.Telephone, _viewModel.PhoneNumbers);
            new AddMoreBasicElementCell<Email>(_tableView, _emailSection, c_emailTypes, "Enter number", Keyboard.Email, _viewModel.Emails);
            new AddMoreBasicElementCell<Url>(_tableView, _urlSection, c_urlTypes, "Enter number", Keyboard.Url, _viewModel.Urls);
            new AddMoreBasicElementCell<InstantMessage>(_tableView, _imSection, c_imTypes, "Enter number", Keyboard.Text, _viewModel.IMs);

            new AddMoreAddressCell(_tableView, _addressSection, _viewModel.Addresses);
            new AddMoreDateCell(_tableView, _specialDateSection, _viewModel.SpecialDates);

            new AddMoreTagCell(_tableView, _tagSection, _viewModel);
            new AddMoreRelationshipCell(_tableView, _relationshipSection, _viewModel);
        }

        private void OnDoneButtonClicked()
        {
            var returnCode = _viewModel.CreateOrUpdateContact();

            if (returnCode == 0) // Update
            {
                MessagingCenter.Send<AddEditContactPage, Contact>(this, "Update", _viewModel.Contact); 

                // Update the contactDetailPage by add a new (updated) one and remove 2 used ones. A bit clumsy!! SO HARDCODED!! Need fix asap!!
                var enumerator = Navigation.NavigationStack.GetEnumerator();
                enumerator.MoveNext(); // Pass loading screen
                enumerator.MoveNext(); // Pass all contacts (root) page
                enumerator.MoveNext();
                var contactDetailsPage = enumerator.Current;
                enumerator.MoveNext();
                var addEditContactPage = enumerator.Current;

                Navigation.InsertPageBefore(new ContactDetailsPage(_viewModel.Contact, true), contactDetailsPage);
                Navigation.RemovePage(addEditContactPage);
                Navigation.RemovePage(contactDetailsPage);

                // Altenative
//                Navigation.PopToRootAsync();
            }
            else // Create new
            {
                _viewModel.CreateAutoAddedTags();
                MessagingCenter.Send<AddEditContactPage, Contact>(this, "Add", _viewModel.Contact); 

                // Update the contactDetailPage by add a new (updated) one and remove 2 used ones. A bit clumsy!!
                var enumerator = Navigation.NavigationStack.GetEnumerator();
                enumerator.MoveNext(); // Pass loading screen
                enumerator.MoveNext(); // Pass all contacts (root) page
                enumerator.MoveNext();
                var addNewContactPage = enumerator.Current;

                Navigation.InsertPageBefore(new ContactDetailsPage(_viewModel.Contact, true), addNewContactPage);
                Navigation.RemovePage(addNewContactPage);
            }
        }

        /// <summary>
        /// Gets the next entry text. The entry should be the last element of the StackLayout.
        /// </summary>
        /// <returns>The next entry text.</returns>
        /// <param name="enumerator">Enumerator.</param>
        private string GetNextEntryText(IEnumerator<Cell> enumerator)
        {
            if (enumerator.MoveNext())
            {
                var cell = enumerator.Current;
                var view = ((ViewCell)cell).View;
                var layout = (StackLayout)view;
                return ((Entry)layout.Children[layout.Children.Count - 1]).Text;
            }
            return null;
        }

        /// <summary>
        /// Retrieves the type value pairs from a basic table section which contain a picker (type) and an entry (value)
        /// </summary>
        /// <returns>The type value pairs.</returns>
        /// <param name="_section">Section.</param>
        private List<Tuple<string, string>> RetrieveTypeValuePairs(TableSection _section)
        {
            var result = new List<Tuple<string, string>>();
            var enumerator = _section.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var cell = enumerator.Current;
                var view = ((ViewCell)cell).View;
                var layout = (StackLayout)view;

                // If not the AddMoreButton
                if (layout.Children.Count == 4)
                {
                    var picker = (Picker)layout.Children[1];
                    var type = (picker.SelectedIndex != -1) ? picker.Items[picker.SelectedIndex] : "";

                    var entry = (Entry)layout.Children[3];
                    var value = entry.Text;

                    result.Add(new Tuple<string, string>(type, value));
                }
            }
            return result;
        }
    }
}