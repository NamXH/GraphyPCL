using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class UserDataNavigationPage : NavigationPage
    {
        public UserDataNavigationPage()
        {
            Title = "Summary";
            Icon = "genius_icon.png";

            // Do this to avoid making properties of UserDataPage notifyPropertyChanged (because too tedious with 42 props)
            var blankPage = new ContentPage
            {
                Title = " ",
            };
            blankPage.ToolbarItems.Add(new ToolbarItem("Calculate", null, () =>
                    {
                        blankPage.Navigation.PushAsync(new UserDataPage());
                    }));

            this.PushAsync(blankPage);
        }
    }
}