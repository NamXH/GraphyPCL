using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreDateCell : AddMoreElementCell
    {
        protected const string c_defaultDateFormat = "MMM d yyyy";

        public AddMoreDateCell(ExtendedTableView table, TableSection tableSection)
            : base()
        {
            ContainerTable = table;
            ContainerSection = tableSection;

            this.Types = new List<string>() { "anniversary", "other" };

            CreateAddMoreButton();
        }

        protected override void AddMoreElement(object sender, EventArgs args)
        {
            var viewCell = new ViewCell();
            ContainerSection.Insert(ContainerSection.Count - 1, viewCell);

            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = _defaulPadding
            };
            viewCell.View = layout;

            var deleteImage = new Image();
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            deleteTapped.Tapped += (s, e) =>
            {
                ContainerSection.Remove(viewCell);
                ContainerTable.OnDataChanged();
            };
            layout.Children.Add(deleteImage);

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

            var seperator = new BoxView();
            layout.Children.Add(seperator);
            seperator.Color = Color.Gray;
            seperator.WidthRequest = 1;
            seperator.HeightRequest = layout.Height;

            var datePicker = new DatePicker
            {
                Format = c_defaultDateFormat,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(datePicker);

            ContainerTable.OnDataChanged();
        }
    }
}