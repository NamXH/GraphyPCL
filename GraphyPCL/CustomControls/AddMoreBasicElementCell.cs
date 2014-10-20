using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreBasicElementCell<T> : AddMoreElementCell where T : IIdContainer, IContactIdRelated, ITypeValuePairContainer, new() 
    {
        protected const double c_entryWidth = 75;

        public IList<string> Types { get; set; }

        public string EntryPlaceHolder { get; set; }

        public Keyboard EntryKeyboardType { get; set; }

        public IList<T> Items { get; set; }

        /// <summary>
        /// Note: A table section can only have 1 AddMoreElementCell
        /// </summary>
        /// <param name="containerTableSection">Container table section.</param>
        public AddMoreBasicElementCell(ExtendedTableView table, TableSection tableSection, IList<string> types, string entryPlaceHolder, Keyboard entryKeyboardType, IList<T> items)
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

            Items = items;
            foreach (var item in Items)
            {
                CreateNewCell(item);
            }
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var item = new T();
            item.Id = Guid.NewGuid();
            Items.Add(item);
            CreateNewCell(item);
        }

        private void CreateNewCell(T item)
        {
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

            var picker = new Picker
                {
                    BindingContext = item,
                    Title = "type",
                    WidthRequest = c_entryWidth,
                    BackgroundColor = Device.OnPlatform(Color.Silver, Color.Default, Color.Default),
                };
            foreach (var type in Types)
            {
                picker.Items.Add(type);
            }
            picker.SetBinding(Picker.SelectedIndexProperty, new Binding("Type", BindingMode.TwoWay, new PickerStringToIntConverter(), Types));
            layout.Children.Add(picker);

            //            var seperator = new BoxView();
            //            layout.Children.Add(seperator);
            //            seperator.Color = Color.Gray;
            //            seperator.WidthRequest = 1;
            //            seperator.HeightRequest = layout.Height;

            var entry = new Entry();
            entry.BindingContext = item;
            layout.Children.Add(entry);
            entry.Placeholder = EntryPlaceHolder;
            entry.Keyboard = EntryKeyboardType;
            entry.HorizontalOptions = LayoutOptions.FillAndExpand;
            entry.SetBinding(Entry.TextProperty, "Value", BindingMode.TwoWay);

            ContainerTable.OnDataChanged();
        }

        private class PickerStringToIntConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var itemToFind = (string)value;
                var itemList = (List<string>)parameter;
                return itemList.FindIndex(x => x == itemToFind);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var index = (int)value;
                if (index == -1)
                {
                    return null;
                }
                else
                {
                    var itemList = (List<string>)parameter;
                    return itemList[index];
                }
            }
        }
    }
}