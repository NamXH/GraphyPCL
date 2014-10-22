using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreDateCell : AddMoreElementCell
    {
        protected const double c_entryWidth = 75;
        protected const string c_defaultDateFormat = "MMM d yyyy";

        public IList<string> Types { get; set; }

        public IList<SpecialDate> Items { get; set; }

        public AddMoreDateCell(ExtendedTableView table, TableSection tableSection, IList<SpecialDate> items)
            : base(table, tableSection)
        {
            this.Types = new List<string>() { "birthday", "anniversary", "other" }; // Hard coded!!
            Items = items;
            foreach (var item in items)
            {
                CreateCell(item);
            }
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var date = new SpecialDate();
            date.Id = Guid.NewGuid();
            Items.Add(date);
            CreateCell(date);
        }

        private void CreateCell(SpecialDate date)
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
            deleteImage.BindingContext = date;
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            deleteTapped.Tapped += (s, e) =>
                {
                    Items.Remove(date);
                    ContainerSection.Remove(viewCell);
                    ContainerTable.OnDataChanged();
                };
            layout.Children.Add(deleteImage);

            var picker = new Picker
                {
                    BindingContext = date,
                    Title = "type",
                    WidthRequest = c_entryWidth,
                    BackgroundColor = Device.OnPlatform(Color.Silver, Color.Default, Color.Default),
                };
            foreach (var item in Types)
            {
                picker.Items.Add(item);
            }
            picker.SetBinding(Picker.SelectedIndexProperty, new Binding("Type", BindingMode.TwoWay, new PickerStringToIntConverter(), Types));
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
            datePicker.SetBinding<SpecialDate>(DatePicker.DateProperty, x => x.Date, BindingMode.TwoWay);
            layout.Children.Add(datePicker);

            ContainerTable.OnDataChanged();
        }
    }
}