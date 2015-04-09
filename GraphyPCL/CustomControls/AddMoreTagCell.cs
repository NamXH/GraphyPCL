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
            foreach (var completeTag in ViewModel.CompleteTags)
            {
                CreateCell(completeTag);
            }
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var completeTag = new CompleteTag();
            completeTag.Id = Guid.NewGuid();
            ViewModel.CompleteTags.Add(completeTag);
            CreateCell(completeTag);
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

        private void CreateCell(CompleteTag completeTag)
        {
            // Tag
            var tagViewCell = InsertViewWithLayout(false);
            var tagLayout = (StackLayout)tagViewCell.View;

            var deleteImage = new Image();
            deleteImage.BindingContext = completeTag;
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
                    BindingContext = completeTag,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Title = "Pick a tag"
                };
            foreach (var tag in ViewModel.Tags)
            {
                tagPicker.Items.Add(tag.Name);
            }
            tagPicker.SetBinding(Picker.SelectedIndexProperty, new Binding("Id", BindingMode.TwoWay, new PickerGuidToIntConverter<Tag>(), ViewModel.Tags));
            tagPicker.SelectedIndex = 0;
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
                    BindingContext = completeTag,
                    Placeholder = "Enter tag detail",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
            detailEntry.SetBinding(Entry.TextProperty, "Detail", BindingMode.TwoWay);
            detailLayout.Children.Add(detailEntry);

            var newTagViewCell = InsertViewWithLayout(true);
            var labelLayout = (StackLayout)newTagViewCell.View;

            var newTagEntry = new Entry
                {
                    BindingContext = completeTag,
                    Placeholder = "Create a new tag name",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
            newTagEntry.SetBinding(Entry.TextProperty, new Binding("NewTagName", BindingMode.TwoWay));
            labelLayout.Children.Add(newTagEntry);

            // Delete action
            deleteTapped.Tapped += (s, e) =>
                {
                    ViewModel.CompleteTags.Remove(completeTag);
                    ContainerSection.Remove(tagViewCell);
                    ContainerSection.Remove(detailViewCell);
                    ContainerSection.Remove(newTagViewCell);
                    ContainerTable.OnDataChanged();
                };

            ContainerTable.OnDataChanged();
        }
    }
}