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

        private ContactDetailsViewModel _viewModel;

        public AddContactPage()
        {
            InitializeComponent();

            _tableView.Intent = TableIntent.Menu;
            this.ToolbarItems.Add(new ToolbarItem("Done", null, OnDoneButtonClicked));

            _viewModel = new ContactDetailsViewModel();
            BindingContext = _viewModel;

            _phoneSection.Add(new AddMoreElementCell(_tableView, _phoneSection, c_phoneTypes, "Enter number", Keyboard.Telephone));
            _emailSection.Add(new AddMoreElementCell(_tableView, _emailSection, c_emailTypes, "Enter email", Keyboard.Email));
            _urlSection.Add(new AddMoreElementCell(_tableView, _urlSection, c_urlTypes, "Enter url", Keyboard.Url));
            _imSection.Add(new AddMoreElementCell(_tableView, _imSection, c_imTypes, "Enter nickname", Keyboard.Text));
        }


        private void OnDoneButtonClicked()
        {
        }
    }
}