using System;

using Xamarin.Forms;

namespace GraphyPCL
{
    public class BasicSearchNavigationPage : ContentPage
    {
        public BasicSearchNavigationPage()
        {
            Content = new StackLayout
            { 
                Children =
                {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}


