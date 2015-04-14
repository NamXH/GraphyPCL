using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreRelationshipCriteriaCell : AddMoreElementCell
    {
        private const double c_labelWidth = 80;
        private const double c_deleteIconWidth = 29;
        private const double c_arrowButtonWidth = 70;

        public IList<CompleteRelationship> CompleteRelationships { get; set; }

        public AddMoreRelationshipCriteriaCell(ExtendedTableView table, TableSection tableSection, IList<CompleteRelationship> completeRelationships)
            : base(table, tableSection)
        {
            CompleteRelationships = completeRelationships;

            foreach (var completeRelationship in CompleteRelationships)
            {
                CreateNewCell(completeRelationship);
            }
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var completeRelationship = new CompleteRelationship();
            CompleteRelationships.Add(completeRelationship);
            CreateNewCell(completeRelationship);
        }

        // A part of this function can go into the base class !!
        private void CreateNewCell(CompleteRelationship completeRelationship)
        {
            #region Relationship type name region
            var nameCell = InsertViewWithLayout(false);
            var nameLayout = (StackLayout)nameCell.View;

            var deleteImage = new Image
            {
                Source = ImageSource.FromFile("minus_icon.png"),
                BindingContext = completeRelationship
            };
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped);
            nameLayout.Children.Add(deleteImage);

            var nameLabel = new Label
            {
                Text = "Rel. Name",
                WidthRequest = c_labelWidth,
                VerticalOptions = LayoutOptions.Center
            };
            nameLayout.Children.Add(nameLabel);

            var nameEntry = new Entry();
            nameEntry.BindingContext = completeRelationship;
            nameLayout.Children.Add(nameEntry);
            nameEntry.HorizontalOptions = LayoutOptions.FillAndExpand;
            nameEntry.SetBinding(Entry.TextProperty, "RelationshipTypeName", BindingMode.TwoWay);
            #endregion

            #region Relationship direction region
            var directionCell = InsertViewWithLayout(true);
            var directionLayout = (StackLayout)directionCell.View;

            var directionLabel = new Label
            {
                Text = "Direction",
                WidthRequest = c_labelWidth,
                VerticalOptions = LayoutOptions.Center
            };
            directionLayout.Children.Add(directionLabel);

            var arrow = new Button
            { 
                WidthRequest = c_arrowButtonWidth,
                BackgroundColor = Color.Silver,
                BindingContext = completeRelationship
            };
            arrow.Clicked += (s, e) =>
            {
                if (arrow.Text == "=>")
                {
                    arrow.Text = "<=";
                }
                else
                {
                    arrow.Text = "=>";
                }
            };
            arrow.SetBinding(Button.TextProperty, "IsToRelatedContact", BindingMode.TwoWay, new ArrowButtonBoolToStringConverter());
            directionLayout.Children.Add(arrow); 

            #endregion

            // Delete Action
            deleteTapped.Tapped += (s, e) =>
            {
                CompleteRelationships.Remove(completeRelationship);
                ContainerSection.Remove(nameCell);
                ContainerSection.Remove(directionCell);
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