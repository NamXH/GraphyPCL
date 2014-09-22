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

//            var foo = new TextCell();
//            var a = new TextCell{ Text = "abc" };
//            _phoneSection.Insert(_phoneSection.Count - 1, a);
        }

        private void OnAddMoreButtonClick(object sender, EventArgs args)
        {
            var a = new TextCell{ Text = "abc" };
            _phoneSection.Insert(_phoneSection.Count - 1, a);
            _tableView.OnDataChanged();
        }

        private void OnDoneButtonClick()
        {
        }
    }
}