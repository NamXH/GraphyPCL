using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class BasicSearchNavigationPage : NavigationPage
    {
        public BasicSearchNavigationPage()
        {
            Icon = "magnifying_glass_icon.png";
            Title = "Search";
            this.PushAsync(new BasicSearchPage());
        }
    }
}