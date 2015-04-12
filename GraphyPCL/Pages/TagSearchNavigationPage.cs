using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class TagSearchNavigationPage : NavigationPage
    {
        public TagSearchNavigationPage()
        {
            Icon = "tag_icon.png";
            Title = "Tags";
            this.PushAsync(new Page());
        }
    }
}