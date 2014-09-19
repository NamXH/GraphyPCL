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
            this.ToolbarItems.Add(new ToolbarItem("Done", null, () =>
                    {
                    }));

            _viewModel = new ContactDetailsViewModel();
            BindingContext = _viewModel;
        }
    }
}