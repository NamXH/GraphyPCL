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
            _phoneSection.Insert(_phoneSection.Count - 1, viewCell);

            var layout = new StackLayout();
            viewCell.View = layout;
            layout.Orientation = StackOrientation.Horizontal;
            layout.Padding = new Thickness(20, 5, 20, 5);

            var deleteImage = new Image();
            layout.Children.Add(deleteImage);
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");

            var label = new Label();
            layout.Children.Add(label);
            label.Text = "home";
            label.VerticalOptions = LayoutOptions.CenterAndExpand;
            label.WidthRequest = 70;
            var labelTapped = new TapGestureRecognizer();
            label.GestureRecognizers.Add(labelTapped);
            labelTapped.Tapped += (object s, EventArgs e) =>
            {
                var a = new ElementTypePage();
                this.Navigation.PushAsync(a);
            };

            var box = new BoxView();
            layout.Children.Add(box);
            box.Color = Color.Gray;
            box.WidthRequest = 1;
            box.HeightRequest = layout.Height;

            var entry = new Entry();
            layout.Children.Add(entry);
            entry.Placeholder = "Enter number";
            entry.Keyboard = Keyboard.Numeric;
            entry.HorizontalOptions = LayoutOptions.FillAndExpand;

            _tableView.OnDataChanged();
        }

        private void OnDoneButtonClick()
        {
        }
    }
}