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

        private void AddMorePhoneNumber(object sender, EventArgs args)
        {
            var viewCell = new ViewCell();

            var layout = new StackLayout();
            layout.Orientation = StackOrientation.Horizontal;
            layout.Padding = new Thickness(20, 5, 20, 5);
            viewCell.View = layout;

            var deleteImage = new Image();
            deleteImage.HeightRequest = 12;
            deleteImage.WidthRequest = 12;
            deleteImage.Source = ImageSource.FromFile("plus_icon.png");
            layout.Children.Add(deleteImage);

            var label = new Label();
            label.Text = "Home";
            label.VerticalOptions = LayoutOptions.CenterAndExpand;
            label.WidthRequest = 70;
            layout.Children.Add(label);

            var box = new BoxView();
            box.Color = Color.Gray;
            box.WidthRequest = 1;
            box.HeightRequest = layout.Height;
            layout.Children.Add(box);

            var entry = new Entry();
            entry.Placeholder = "Enter number";
            entry.HorizontalOptions = LayoutOptions.EndAndExpand;
            entry.Keyboard = Keyboard.Numeric;
            layout.Children.Add(entry);

            _phoneSection.Insert(_phoneSection.Count - 1, viewCell);

            _tableView.OnDataChanged();
        }

        private void OnDoneButtonClick()
        {
        }
    }
}