using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class FullSearchNavigationPage : NavigationPage 
    {
        public FullSearchNavigationPage()
        {
            Icon = "magnifying_glass_icon.png";
            Title = "Search";
            this.PushAsync(new FullSearchPage());
        }
    }
}