using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class AddContactPage : ContentPage
    {
        private ContactDetailsViewModel _viewModel;

        public AddContactPage()
        {
            InitializeComponent();

            _tableView.Intent = TableIntent.Menu;
            this.ToolbarItems.Add(new ToolbarItem("Done", null, OnDoneButtonClick));

            _viewModel = new ContactDetailsViewModel();
            BindingContext = _viewModel;

            var phoneLabels = new List<string>() { "home", "work", "mobile", "other" };
            _phoneSection.Add(new AddMoreElementCell(_tableView, _phoneSection, phoneLabels, "Enter phone number", Keyboard.Numeric));
        }



        private void OnDoneButtonClick()
        {
        }
    }
}