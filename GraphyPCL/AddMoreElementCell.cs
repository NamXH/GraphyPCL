using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreElementCell : ViewCell 
    {
        private readonly Color _iOSBlue = Color.FromRgb(0, 122, 255);

        public AddMoreElementCell()
        {
            var layout = new StackLayout();
            layout.Orientation = StackOrientation.Horizontal;
            layout.Padding = new Thickness(20, 5, 20, 5);
            this.View = layout;

            var plusImage = new Image();
            plusImage.Source = ImageSource.FromFile("plus_circled_icon.png");
            layout.Children.Add(plusImage);

            var label = new Label();
            label.Text = "add more";
            label.TextColor = Device.OnPlatform(_iOSBlue, Color.Default, Color.Default);
            label.VerticalOptions = LayoutOptions.CenterAndExpand;
            layout.Children.Add(label);
        }
    }
}