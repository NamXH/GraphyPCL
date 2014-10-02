using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class AddContactPage : ContentPage
    {
        private List<string> c_phoneTypes = new List<string>() { "mobile", "home", "work", "main", "other" };
        private List<string> c_emailTypes = new List<string>() { "home", "work", "main", "other" };
        private List<string> c_urlTypes = new List<string>() { "home", "work", "main", "other" };
        private List<string> c_imTypes = new List<string>() { "skype", "hangouts", "facebook", "msn", "yahoo", "aim", "qq", "other" };

        private ContactViewModel _viewModel;

        public AddContactPage()
        {
            InitializeComponent();

            _tableView.Intent = TableIntent.Menu;
            this.ToolbarItems.Add(new ToolbarItem("Done", null, OnDoneButtonClicked));

            _viewModel = new ContactViewModel(); // Caution: Contact.Birthday is 1/1/0001 here!!
            BindingContext = _viewModel; // Caution: DatePicker automatically set the default Contact.Birthday to 1/1/1900 here!!

            _phoneSection.Add(new AddMoreElementCell(_tableView, _phoneSection, c_phoneTypes, "Enter number", Keyboard.Telephone));
            _emailSection.Add(new AddMoreElementCell(_tableView, _emailSection, c_emailTypes, "Enter email", Keyboard.Email));
            _urlSection.Add(new AddMoreElementCell(_tableView, _urlSection, c_urlTypes, "Enter url", Keyboard.Url));
            _imSection.Add(new AddMoreElementCell(_tableView, _imSection, c_imTypes, "Enter nickname", Keyboard.Text));

            _addressSection.Add(new AddMoreAddressCell(_tableView, _addressSection));
            _specialDateSection.Add(new AddMoreDateCell(_tableView, _specialDateSection));
        }


        private void OnDoneButtonClicked()
        {
            // Save unbindable fields to view model
            _viewModel.PhoneNumbers = new List<PhoneNumber>();
            var phoneNumbers = RetrieveTypeValuePairs(_phoneSection);
            foreach (var phoneNumber in phoneNumbers)
            {
                _viewModel.PhoneNumbers.Add(new PhoneNumber
                    {
                        Type = phoneNumber.Item1,
                        Number = phoneNumber.Item2
                    });
            }

            _viewModel.Emails = new List<Email>();
            var emails = RetrieveTypeValuePairs(_emailSection);
            foreach (var email in emails)
            {
                _viewModel.Emails.Add(new Email
                    {
                        Type = email.Item1,
                        Address = email.Item2
                    });
            }

            _viewModel.Urls = new List<Url>();
            var urls = RetrieveTypeValuePairs(_urlSection);
            foreach (var url in urls)
            {
                _viewModel.Urls.Add(new Url
                    {
                        Type = url.Item1,
                        Link = url.Item2
                    });
            }

            _viewModel.IMs = new List<InstantMessage>();
            var ims = RetrieveTypeValuePairs(_imSection);
            foreach (var im in ims)
            {
                _viewModel.IMs.Add(new InstantMessage
                    {
                        Type = im.Item1,
                        Nickname = im.Item2
                    });
            }

            // Need refactor, too hard-coded!!
            _viewModel.Addresses = new List<Address>();
            var addressEnumerator = _addressSection.GetEnumerator();
            while (addressEnumerator.MoveNext())
            {
                var cell = addressEnumerator.Current;
                var view = ((ViewCell)cell).View;
                var layout = (StackLayout)view;

                // If it is the first entry (streetline 1) of the address
                if ((layout.Children.Count == 2) && (layout.Children[1].GetType() == typeof(Entry)))
                {
                    var address = new Address();
                    address.StreetLine1 = ((Entry)layout.Children[1]).Text;

                    // Get 5 next entries
                    address.StreetLine2 = GetNextEntryText(addressEnumerator);
                    address.City = GetNextEntryText(addressEnumerator);
                    address.Province = GetNextEntryText(addressEnumerator);
                    address.Country = GetNextEntryText(addressEnumerator);
                    address.PostalCode = GetNextEntryText(addressEnumerator);

                    _viewModel.Addresses.Add(address);
                }
            }

            // Need refactor too!!
            _viewModel.SpecialDates = new List<SpecialDate>();
            var dateEnumerator = _specialDateSection.GetEnumerator();
            while (dateEnumerator.MoveNext())
            {
                var cell = dateEnumerator.Current;
                var view = ((ViewCell)cell).View;
                var layout = (StackLayout)view;

                if (layout.Children[layout.Children.Count - 1].GetType() == typeof(DatePicker))
                {
                    var picker = (Picker)layout.Children[1];
                    var type = (picker.SelectedIndex != -1) ? picker.Items[picker.SelectedIndex] : "";

                    var datePicker = (DatePicker)layout.Children[3];
                    var date = datePicker.Date;

                    _viewModel.SpecialDates.Add(new SpecialDate
                        {
                            Type = type,
                            Date = date
                        });
                }
            }
        }

        private string GetNextEntryText(IEnumerator<Cell> enumerator)
        {
            if (enumerator.MoveNext())
            {
                var cell = enumerator.Current;
                var view = ((ViewCell)cell).View;
                var layout = (StackLayout)view;
                return ((Entry)layout.Children[0]).Text;
            }
            return null;
        }

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