using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreElementCell : ViewCell
    {
        private readonly Color _iOSBlue = Color.FromRgb(0, 122, 255);
        private readonly Thickness _defaulPadding = new Thickness(20, 5, 20, 5);
        private const double c_entryWidth = 75;

        public TableSection ContainerSection { get; set; }

        public ExtendedTableView ContainerTable { get; set; }

        public IList<string> Types { get; set; }

        public string EntryPlaceHolder { get; set; }

        public Keyboard EntryKeyboardType { get; set; }

        /// <summary>
        /// Note: A table section can only have 1 AddMoreElementCell
        /// </summary>
        /// <param name="containerTableSection">Container table section.</param>
        public AddMoreElementCell(ExtendedTableView table, TableSection tableSection, IList<string> types, string entryPlaceHolder, Keyboard entryKeyboardType)
            : base()
        {
            ContainerTable = table;
            ContainerSection = tableSection;
            if ((types == null) || (types.Count == 0))
            {
                throw new Exception("The labelsList should not be null or empty");
            }
            Types = types;
            EntryPlaceHolder = entryPlaceHolder;
            if (entryKeyboardType == null)
            {
                throw new Exception("The entryKeyboardType should not be null");
            }
            EntryKeyboardType = entryKeyboardType;

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

            this.Tapped += CellTapped;
        }

        private void CellTapped(object sender, EventArgs args)
        {
            var viewCell = new ViewCell();
            ContainerSection.Insert(ContainerSection.Count - 1, viewCell);
        
            var layout = new StackLayout();
            viewCell.View = layout;
            layout.Orientation = StackOrientation.Horizontal;
            layout.Padding = _defaulPadding;
        
            var deleteImage = new Image();
            layout.Children.Add(deleteImage);
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            deleteTapped.Tapped += (s, e) =>
            {
                ContainerSection.Remove(viewCell);
                ContainerTable.OnDataChanged();
            };
        
            var picker = new Picker
            {
                Title = "type",
                WidthRequest = c_entryWidth,
                BackgroundColor = Device.OnPlatform(Color.Silver, Color.Default, Color.Default),
            };
            foreach (var item in Types)
            {
                picker.Items.Add(item);
            }
            layout.Children.Add(picker);
        
            var seperator = new BoxView();
            layout.Children.Add(seperator);
            seperator.Color = Color.Gray;
            seperator.WidthRequest = 1;
            seperator.HeightRequest = layout.Height;
        
            var entry = new Entry();
            layout.Children.Add(entry);
            entry.Placeholder = EntryPlaceHolder;
            entry.Keyboard = EntryKeyboardType;
            entry.HorizontalOptions = LayoutOptions.FillAndExpand;
        
            ContainerTable.OnDataChanged();
        }
    }
}