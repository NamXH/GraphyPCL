using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreElementCell : ViewCell
    {
        protected readonly Color _iOSBlue = Color.FromRgb(0, 122, 255);
        protected readonly Thickness _defaulPadding = new Thickness(20, 5, 20, 5);

        public TableSection ContainerSection { get; set; }

        public ExtendedTableView ContainerTable { get; set; }

        protected AddMoreElementCell()
            : base()
        {
        }

        public AddMoreElementCell(ExtendedTableView table, TableSection tableSection)
            : base()
        {
            ContainerTable = table;
            ContainerSection = tableSection;

            CreateAddMoreButton();
            this.Tapped += OnCellClicked;
        }

        /// <summary>
        /// Creates the add more button: an image of a plus sign and a label.
        /// </summary>
        protected virtual void CreateAddMoreButton()
        {
            var layout = new StackLayout();
            this.View = layout;
            layout.Orientation = StackOrientation.Horizontal;
            layout.Padding = _defaulPadding;

            var plusImage = new Image();
            layout.Children.Add(plusImage);
            plusImage.Source = ImageSource.FromFile("plus_circled_icon.png");

            var label = new Label();
            layout.Children.Add(label);
            label.Text = "add more";
            label.TextColor = Device.OnPlatform(_iOSBlue, Color.Default, Color.Default);
            label.VerticalOptions = LayoutOptions.CenterAndExpand;
        }

        /// <summary>
        /// Implement this method to define what happens when the AddMoreElementCell is clicked.
        /// Default to do nothing.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        protected virtual void OnCellClicked(object sender, EventArgs args)
        {
        }
    }
}