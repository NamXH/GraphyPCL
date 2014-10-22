using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreAddressCell : AddMoreElementCell
    {
        protected const double c_entryWidth = 75;
        protected const double c_leftIndent = 109;

        public IList<string> Types { get; set; }

        public IList<Address> Items { get; set; }

        public AddMoreAddressCell(ExtendedTableView table, TableSection tableSection, IList<Address> items)
            : base(table, tableSection)
        {
            this.Types = new List<string>() { "home", "work", "other" }; // Hard coded!!
            Items = items;
            foreach (var item in Items)
            {
                CreateCell(item);
            }
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var address = new Address();
            address.Id = Guid.NewGuid();
            Items.Add(address);
            CreateCell(address);
        }

        private void CreateCell(Address address)
        {
            var street1 = InsertNewEntry("Street Address 1", address, x => x.StreetLine1, false);
            var street2 = InsertNewEntry("Street Address 2", address, x => x.StreetLine2);
            var city = InsertNewEntry("City", address, x => x.City);
            var province = InsertNewEntry("Province", address, x => x.Province);
            var country = InsertNewEntry("Country", address, x => x.Country);
            var postal = InsertNewEntry("Postal Code", address, x => x.PostalCode);

            var deleteImage = new Image();
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            deleteImage.BindingContext = address;
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            deleteTapped.Tapped += (s, e) =>
            {
                Items.Remove(address);
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
                BindingContext = address,
                Title = "type",
                WidthRequest = c_entryWidth,
                BackgroundColor = Device.OnPlatform(Color.Silver, Color.Default, Color.Default),
            };
            foreach (var item in Types)
            {
                picker.Items.Add(item);
            }
            picker.SetBinding(Picker.SelectedIndexProperty, new Binding("Type", BindingMode.TwoWay, new PickerStringToIntConverter(), Types));

            var layout = (StackLayout)street1.View;
            layout.Children.Insert(0, deleteImage);
            layout.Children.Insert(1, picker);

            ContainerTable.OnDataChanged();
        }

        protected ViewCell InsertNewEntry(string placeholderText, Address address, Expression<Func<Address, object>> sourceProperty, bool leftIndent = true)
        {
            var padding = _defaulPadding;
            if (leftIndent)
            {
                padding.Left += c_leftIndent;
            }

            var entry = new Entry
            {
                BindingContext = address,
                Placeholder = placeholderText,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            entry.SetBinding<Address>(Entry.TextProperty, sourceProperty, BindingMode.TwoWay);

            var viewCell = new ViewCell
            {
                View = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = padding,
                    Children = { entry }
                }
            };

            ContainerSection.Insert(ContainerSection.Count - 1, viewCell);

            return viewCell;
        }
    }
}