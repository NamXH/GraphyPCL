using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreAddressCell : AddMoreElementCell
    {
        protected const double c_leftIndent = 27;

        public AddMoreAddressCell(ExtendedTableView table, TableSection tableSection)
            : base()
        {
            ContainerTable = table;
            ContainerSection = tableSection;

            CreateAddMoreButton();
        }

        protected override void AddMoreElement(object sender, EventArgs args)
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
            var layout = (StackLayout)street1.View;
            layout.Children.Insert(0, deleteImage);

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