using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AllContactsNavigationPage : NavigationPage
    {
        public AllContactsNavigationPage()
        {
            Icon = "contact_icon.png";
            Title = "Contacts";

            // The loading screen appear in case the DB takes to long to initiate
            var loadingScreen = new ContentPage
            {
                Title = "Loading",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Text = "Setting up for the first time use"
                        },

                        new ActivityIndicator
                        {
                            IsRunning = true
                        },
                    }
                }
            };

            // For test
//            var button = new Button
//                {
//                    Text = "Click",
//                };
//            button.Clicked += (object sender, EventArgs e) => 
//                {
//                    CreateAllContactsPage();
//                };
//            var load = new ContentPage();
//            load.Title = "Loading";
//            var layout = new StackLayout();
//            load.Content = layout;
//            layout.Children.Add(button);
//            this.PushAsync(load);


            this.PushAsync(loadingScreen);

            CreateAllContactsPage();

            GetCurrentLocation();
        }

        private async void CreateAllContactsPage()
        {
            await DatabaseManager.Initialize();
            var allContacts = DatabaseManager.GetRows<Contact>();
            var allContactsPage = new AllContactsPage(allContacts);
            SetHasBackButton(allContactsPage, false); // Prevent jumping back to the loading screen
            this.PushAsync(allContactsPage);
        }

        private async void GetCurrentLocation()
        {
            await GeolocationManager.UpdateGeolocation();
        }
    }
}