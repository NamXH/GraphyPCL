using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            _viewModel = new ContactViewModel(); 
            BindingContext = _viewModel;

            _phoneSection.Add(new AddMoreBasicElementCell(_tableView, _phoneSection, c_phoneTypes, "Enter number", Keyboard.Telephone));
            _emailSection.Add(new AddMoreBasicElementCell(_tableView, _emailSection, c_emailTypes, "Enter email", Keyboard.Email));
            _urlSection.Add(new AddMoreBasicElementCell(_tableView, _urlSection, c_urlTypes, "Enter url", Keyboard.Url));
            _imSection.Add(new AddMoreBasicElementCell(_tableView, _imSection, c_imTypes, "Enter nickname", Keyboard.Text));

            _addressSection.Add(new AddMoreAddressCell(_tableView, _addressSection));
            _specialDateSection.Add(new AddMoreDateCell(_tableView, _specialDateSection));

            _tagSection.Add(new AddMoreTagCell(_tableView, _tagSection, _viewModel));
            _relationshipSection.Add(new AddMoreRelationshipCell(_tableView, _relationshipSection, _viewModel));
        }


        private void OnDoneButtonClicked()
        {
            #region Save unbindable fields to view model

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
                if ((layout.Children.Count == 3) && (layout.Children[2].GetType() == typeof(Entry)))
                {
                    var address = new Address();
                    var picker = (Picker)layout.Children[1];
                    address.Type = picker.Items[picker.SelectedIndex];
                    address.StreetLine1 = ((Entry)layout.Children[2]).Text;

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

            _viewModel.ContactTagMaps = new List<ContactTagMap>();
            var tagEnumerator = _tagSection.GetEnumerator();
            while (tagEnumerator.MoveNext())
            {
                var cell = tagEnumerator.Current;
                var view = ((ViewCell)cell).View;
                var layout = (StackLayout)view;

                if (((layout.Children.Count == 3)) && (layout.Children[0].GetType() == typeof(Image)))
                {
                    var picker = (Picker)layout.Children[2];
                    var pickedTag = (picker.SelectedIndex != -1) ? picker.Items[picker.SelectedIndex] : "";

                    var detail = GetNextEntryText(tagEnumerator);
                    var newTagName = GetNextEntryText(tagEnumerator);

                    if (String.IsNullOrEmpty(newTagName))
                    {
                        // Get tag Id
                        var tags = DatabaseManager.GetRowsByName<Tag>(pickedTag);
                        if (tags.Count > 1)
                        {
                            // {Tag.UserId, Tag.Name} compounds a primary key. However, in order to simplify, we still use seperated primary key.
                            throw new Exception("Duplicate tag name: " + tags[0].Name);
                        }
                        var tagId = tags[0].Id;

                        var contactTapMap = new ContactTagMap
                        {
                            TagId = tagId,
                            Detail = detail
                        };
                        _viewModel.ContactTagMaps.Add(contactTapMap);
                    }
                    else
                    {
                        Guid tagId;
                        var alreadyExistTag = _viewModel.Tags.Where(x => x.Name == newTagName);
                        var theNewTagNameAlreadyExistMoreThanOnce = alreadyExistTag.Count() > 1;
                        var theNewTagNameAlreadyExistOnce = alreadyExistTag.Count() == 1;

                        if (theNewTagNameAlreadyExistMoreThanOnce)
                        {
                            throw new Exception("Duplicate tag name: " + alreadyExistTag.First().Name);
                        }
                        if (theNewTagNameAlreadyExistOnce)
                        {
                            tagId = alreadyExistTag.First().Id;
                        }
                        else
                        {
                            // None exists => create new
                            var tag = new Tag
                            {
                                Id = Guid.NewGuid(),
                                Name = newTagName 
                            };
                            _viewModel.Tags.Add(tag);
                            tagId = tag.Id;
                        }

                        var contactTagMap = new ContactTagMap
                        {
                            TagId = tagId,
                            Detail = detail
                        };
                        _viewModel.ContactTagMaps.Add(contactTagMap);
                    }
                }
            }

//            var a = _relationshipSection[0];
//            var b = _relationshipSection[1];
//            var c = _relationshipSection[2];
//            var t = (StackLayout)((ViewCell)a).View;
//            var t1 = t.Children[0];

            #endregion

            _viewModel.SaveNewContact();

            Navigation.PushAsync(new AllContactsPage());
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