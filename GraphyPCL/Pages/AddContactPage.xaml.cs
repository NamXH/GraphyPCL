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


        }

        private void OnAddMoreButtonClick(object sender, EventArgs args)
        {
            _tableView.OnDataChanged();
        }

        private void OnDoneButtonClick()
        {
        }
    }
}