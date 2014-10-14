using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreRelationshipCell : AddMoreElementCell
    {
        private const double c_deleteIconWidth = 29;
        private const double c_buttonWidth = 40;
        private const double c_labelWidth = 46;

        public ContactViewModel ViewModel { get; set; }

        public AddMoreRelationshipCell(ExtendedTableView table, TableSection tableSection, ContactViewModel viewModel)
            : base(table, tableSection)
        {
            ViewModel = viewModel;
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            // Create a new complete relationship
            var completeRelationship = new CompleteRelationship();
            ViewModel.CompleteRelationships.Add(completeRelationship);

            #region 1st row: delete button, arrow, contact name
            var pickContactCell = InsertViewWithLayout(false);
            var pickContactLayout = (StackLayout)pickContactCell.View;

            var deleteImage = new Image
            {
                Source = ImageSource.FromFile("minus_icon.png"),
                BindingContext = completeRelationship
            };
            var deleteTapped = new TapGestureRecognizer();
            deleteImage.GestureRecognizers.Add(deleteTapped); // Not implementing confirmation when delete for fast prototyping!!
            pickContactLayout.Children.Add(deleteImage);

            var arrow = new Button
            { 
                WidthRequest = c_labelWidth,
                BackgroundColor = Color.Silver,
                BindingContext = completeRelationship
            };
            arrow.SetBinding(Button.TextProperty, "IsToRelatedContact", BindingMode.TwoWay, new ArrowButtonBoolToStringConverter());
            pickContactLayout.Children.Add(arrow);

            var contact = new Entry
            {
                Placeholder = "Pick a contact",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BindingContext = completeRelationship
            };
            contact.SetBinding(Entry.TextProperty, new Binding("RelatedContactName", BindingMode.TwoWay));
            pickContactLayout.Children.Add(contact);
            #endregion

            #region 2nd row: relationship name
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
                Title = "Pick a relationship",
                BindingContext = completeRelationship
            };
            relationshipPicker.SetBinding(Picker.SelectedIndexProperty, new Binding("RelationshipTypeId", BindingMode.TwoWay, new PickerGuidToIntConverter(), ViewModel.Tags));
            foreach (var tag in ViewModel.Tags)
            {
                relationshipPicker.Items.Add(tag.Name);
            }
            relationshipNameLayout.Children.Add(relationshipPicker);
            #endregion

            #region 3rd row: detail 
            var detailViewCell = InsertViewWithLayout(true);
            var detailLayout = (StackLayout)detailViewCell.View;

            var detailLabel = new Label
            {
                Text = "Detail",
                WidthRequest = c_labelWidth,
                VerticalOptions = LayoutOptions.Center,
            };
            detailLayout.Children.Add(detailLabel);

            var detailEntry = new Entry
            {
                Placeholder = "Enter relationship detail",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BindingContext = completeRelationship
            };
            detailLabel.SetBinding(Label.TextProperty, "Detail", BindingMode.TwoWay);
            detailLayout.Children.Add(detailEntry);
            #endregion

            #region 4rd row: create new relationship
            var newRelationshipViewCell = InsertViewWithLayout(true);
            var newRelationtionshipLayout = (StackLayout)newRelationshipViewCell.View;

            var newRelationshipEntry = new Entry
            {
                Placeholder = "Create a new relationship",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            newRelationtionshipLayout.Children.Add(newRelationshipEntry);
            #endregion

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

        private class ArrowButtonBoolToStringConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var isToRelatedContact = (bool)value;
                if (isToRelatedContact)
                {
                    return "=>";
                }
                else
                {
                    return "<=";
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var text = (string)value;
                if (text == "<=")
                {
                    return false;
                }
                else
                {
                    // Default to true even the text is not "=>"
                    return true;
                }
            }
        }

        private class PickerGuidToIntConverter : IValueConverter
        {
            #region IValueConverter implementation

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var guid = (Guid)value;
                var tags = (List<Tag>)parameter;
                return tags.FindIndex(x => x.Id == guid);
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
                    var tags = (List<Tag>)parameter;
                    return tags[index].Id;
                }
            }

            #endregion
        }
    }
}