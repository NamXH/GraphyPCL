using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class AddTagPage : ContentPage
    {
        public ContactViewModel ViewModel { get; set; }

        public AddTagPage(ContactViewModel viewModel)
        {
            InitializeComponent();
            this.ToolbarItems.Add(new ToolbarItem("Done", null, OnDoneButtonClicked));

            ViewModel = viewModel;
            this.BindingContext = ViewModel;

            if (ViewModel.Tags.Count == 0)
            {
                _picker.IsEnabled = false;
            }
            else
            {
                foreach (var item in ViewModel.Tags)
                {
                    _picker.Items.Add(item.Name);
                }
                _picker.SelectedIndex = 0;
            }
        }

        private void OnDoneButtonClicked()
        {
            if (!String.IsNullOrEmpty(_newTag.Text))
            {
//                ViewModel.Tags.Add(new Tag
//                    {
//                        Id = Guid.NewGuid(),
//                        Name = _newTag.Text
//                    });

            }
            this.Navigation.PopAsync();
        }
    }
}