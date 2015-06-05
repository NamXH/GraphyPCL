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
                    Children =
                    {new ActivityIndicator
                        {
                            IsRunning = true
                        },
                    }
                }
            };
            this.PushAsync(loadingScreen);

            CreateAllContactsPage();
        }

        private async void CreateAllContactsPage()
        {
            await DatabaseManager.Initialize();
            var allContacts = DatabaseManager.GetRows<Contact>();
            var allContactsPage = new AllContactsPage(allContacts);
            SetHasBackButton(allContactsPage, false); // Prevent jumping back to the loading screen
            this.PushAsync(allContactsPage);
        }
    }
}