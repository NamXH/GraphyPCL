using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreRelationshipCell : AddMoreElementCell
    {
        private const double c_deleteIconWidth = 29;

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
        }

        // Damn, it is a clone!! Seems not right.
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