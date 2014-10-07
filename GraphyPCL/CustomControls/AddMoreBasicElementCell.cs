using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreBasicElementCell : AddMoreElementCell
    {
        protected const double c_entryWidth = 75;

        public IList<string> Types { get; set; }

        public string EntryPlaceHolder { get; set; }

        public Keyboard EntryKeyboardType { get; set; }

        /// <summary>
        /// Note: A table section can only have 1 AddMoreElementCell
        /// </summary>
        /// <param name="containerTableSection">Container table section.</param>
        public AddMoreBasicElementCell(ExtendedTableView table, TableSection tableSection, IList<string> types, string entryPlaceHolder, Keyboard entryKeyboardType)
            : base(table, tableSection)
        {
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
        }

        protected override void OnCellClicked(object sender, EventArgs args)
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
            picker.SelectedIndex = 0;
            layout.Children.Add(picker);

//            var seperator = new BoxView();
//            layout.Children.Add(seperator);
//            seperator.Color = Color.Gray;
//            seperator.WidthRequest = 1;
//            seperator.HeightRequest = layout.Height;

            var entry = new Entry();
            layout.Children.Add(entry);
            entry.Placeholder = EntryPlaceHolder;
            entry.Keyboard = EntryKeyboardType;
            entry.HorizontalOptions = LayoutOptions.FillAndExpand;

            ContainerTable.OnDataChanged();
        }
    }
}