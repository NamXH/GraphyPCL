using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreEntryCell : AddMoreElementCell
    {
        private const double c_labelWidth = 78;

        public IList<StringWrapper> Items { get; set; }

        public AddMoreEntryCell(ExtendedTableView table, TableSection tableSection, IList<StringWrapper> items)
            : base(table, tableSection)
        {
            Items = items;

            foreach (var item in Items)
            {
                CreateNewCell(item);
            }
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var item = new StringWrapper();
            Items.Add(item);
            CreateNewCell(item);
        }

        // A part of this function can go into the base class !!
        private void CreateNewCell(StringWrapper item)
        {
            // This part go into base class !!
            var viewCell = new ViewCell();
            ContainerSection.Insert(ContainerSection.Count - 1, viewCell);

            var layout = new StackLayout();
            viewCell.View = layout;
            layout.Orientation = StackOrientation.Horizontal;
            layout.Padding = _defaulPadding;

            var deleteImage = new Image();
            deleteImage.BindingContext = item;
            layout.Children.Add(deleteImage);
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            deleteTapped.Tapped += (s, e) =>
                {
                    Items.Remove(item);
                    ContainerSection.Remove(viewCell);
                    ContainerTable.OnDataChanged();
                };
            ////////

            var tagLabel = new Label
                {
                    Text = "Tag Name",
                    WidthRequest = c_labelWidth,
                    VerticalOptions = LayoutOptions.Center
                };
            layout.Children.Add(tagLabel);

            var entry = new Entry();
            entry.BindingContext = item;
            layout.Children.Add(entry);
            entry.HorizontalOptions = LayoutOptions.FillAndExpand;
            entry.SetBinding(Entry.TextProperty, "InnerString", BindingMode.TwoWay);

            ContainerTable.OnDataChanged();
        }
    }
}