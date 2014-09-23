using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreElementCell : TextCell
    {
        private readonly Color _iOSBlue = Color.FromRgb(0, 122, 255);

        public AddMoreElementCell()
        {
            this.Text = "add more";
            this.TextColor = Device.OnPlatform(_iOSBlue, Color.Default, Color.Default);
        }
    }
}