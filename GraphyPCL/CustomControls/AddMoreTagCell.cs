using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreTagCell : AddMoreElementCell
    {
        private const double c_labelWidth = 46;
        private const double c_deleteIconWidth = 29;

        public ContactViewModel ViewModel { get; set; }

        public AddMoreTagCell(ExtendedTableView table, TableSection tableSection, ContactViewModel viewModel)
            : base(table, tableSection)
        {
            ViewModel = viewModel;
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            // Tag
            var tagViewCell = InsertViewWithLayout(false);
            var tagLayout = (StackLayout)tagViewCell.View;

            var deleteImage = new Image();
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            tagLayout.Children.Add(deleteImage);

            var tagLabel = new Label
            {
                Text = "Name",
                WidthRequest = c_labelWidth,
                VerticalOptions = LayoutOptions.Center
            };
            tagLayout.Children.Add(tagLabel);

            var tagPicker = new Picker
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Pick a tag"
            };
            foreach (var tag in ViewModel.Tags)
            {
                tagPicker.Items.Add(tag.Name);
            }
            tagLayout.Children.Add(tagPicker);

            // Detail, a bit duplicate!!
            var detailViewCell = InsertViewWithLayout(true);
            var detailLayout = (StackLayout)detailViewCell.View;

            var detailLabel = new Label
            {
                Text = "Detail",
                WidthRequest = c_labelWidth,
                VerticalOptions = LayoutOptions.Center
            };
            detailLayout.Children.Add(detailLabel);

            var detailEntry = new Entry
            {
                Placeholder = "Enter tag detail",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            detailLayout.Children.Add(detailEntry);

            var newTagViewCell = InsertViewWithLayout(true);
            var labelLayout = (StackLayout)newTagViewCell.View;

            var newTagEntry = new Entry
            {
                Placeholder = "Create a new tag name",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            labelLayout.Children.Add(newTagEntry);

            // Delete action
            deleteTapped.Tapped += (s, e) =>
            {
                ContainerSection.Remove(tagViewCell);
                ContainerSection.Remove(detailViewCell);
                ContainerSection.Remove(newTagViewCell);
                ContainerTable.OnDataChanged();
            };

            ContainerTable.OnDataChanged();
        }

        protected virtual ViewCell InsertViewWithLayout(bool leftIndent = false)
        {
            var viewCell = new ViewCell();
            ContainerSection.Insert(ContainerSection.Count - 1, viewCell);

            var padding = _defaulPadding;
            if (leftIndent)
            {
                padding.Left += c_deleteIconWidth;
            }
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = padding
            };
            viewCell.View = layout;

            return viewCell;
        }
    }
}