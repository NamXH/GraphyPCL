using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreAddressCell : AddMoreElementCell
    {
        protected const double c_entryWidth = 75;
        protected const double c_leftIndent = 109;

        public IList<string> Types { get; set; }

        public AddMoreAddressCell(ExtendedTableView table, TableSection tableSection)
            : base(table, tableSection)
        {
            this.Types = new List<string>() { "home", "work", "other" };
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var street1 = InsertNewEntry("Street Address 1", false);
            var street2 = InsertNewEntry("Street Address 2");
            var city = InsertNewEntry("City");
            var province = InsertNewEntry("Province");
            var country = InsertNewEntry("Country");
            var postal = InsertNewEntry("Postal Code");

            var deleteImage = new Image();
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            deleteTapped.Tapped += (s, e) =>
            {
                ContainerSection.Remove(street1);
                ContainerSection.Remove(street2);
                ContainerSection.Remove(city);
                ContainerSection.Remove(province);
                ContainerSection.Remove(country);
                ContainerSection.Remove(postal);
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

            var layout = (StackLayout)street1.View;
            layout.Children.Insert(0, deleteImage);
            layout.Children.Insert(1, picker);

            ContainerTable.OnDataChanged();
        }

        protected ViewCell InsertNewEntry(string placeholderText, bool leftIndent = true)
        {
            var padding = _defaulPadding;
            if (leftIndent)
            {
                padding.Left += c_leftIndent;
            }

            var viewCell = new ViewCell
            {
                View = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = padding,
                    Children =
                    {
                        new Entry
                        {
                            Placeholder = placeholderText,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        }
                    }
                }
            };

            ContainerSection.Insert(ContainerSection.Count - 1, viewCell);

            return viewCell;
        }
    }
}