using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreRelationshipCell : AddMoreElementCell
    {
        private const double c_deleteIconWidth = 29;
        private const double c_buttonWidth = 40;
        private const double c_labelWidth = 46;

        public AddMoreRelationshipCell(ExtendedTableView table, TableSection tableSection)
            : base(table, tableSection)
        {
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var pickContactCell = InsertViewWithLayout(false);
            var pickContactLayout = (StackLayout)pickContactCell.View;

            var deleteImage = new Image();
            deleteImage.Source = ImageSource.FromFile("minus_icon.png");
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            // Not implementing confirmation when delete for fast prototyping!!
            pickContactLayout.Children.Add(deleteImage);

            var arrow = new Button
            { 
                WidthRequest = c_labelWidth,
                BackgroundColor = Color.Silver,
                Text = "=>"
            };
            pickContactLayout.Children.Add(arrow);

            var contact = new Entry
            {
                Placeholder = "Pick a contact",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            pickContactLayout.Children.Add(contact);

            var relationshipNameCell = InsertViewWithLayout(true);
            var relationshipNameLayout = (StackLayout)relationshipNameCell.View;

            var relationshipNameLabel = new Label
            {
                Text = "Name",
                WidthRequest = c_labelWidth,
                VerticalOptions = LayoutOptions.Center
            };
            relationshipNameLayout.Children.Add(relationshipNameLabel);

            var relationshipPicker = new Picker
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Pick a relationship"
            };
//            foreach (var tag in ViewModel.Tags)
//            {
//                relationshipPicker.Items.Add(tag.Name);
//            }
            relationshipNameLayout.Children.Add(relationshipPicker);

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
                    Placeholder = "Enter relationship detail",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
            detailLayout.Children.Add(detailEntry);

            var newRelationshipViewCell = InsertViewWithLayout(true);
            var newRelationtionshipLayout = (StackLayout)newRelationshipViewCell.View;

            var newRelationshipEntry = new Entry
                {
                    Placeholder = "Create a new relationship",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
            newRelationtionshipLayout.Children.Add(newRelationshipEntry);

            ContainerTable.OnDataChanged();
        }

        // Duplicate. Need refactor!!
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