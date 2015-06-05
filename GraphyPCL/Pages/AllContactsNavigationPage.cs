using System;
using System.Collections.Generic;
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
            // Maybe actually don't need the loading screen
            var loadingScreen = new ContentPage
            {
                Content = new StackLayout
                { 
                    Children =
                    {new ActivityIndicator
                        {
                            IsRunning = true
                        },
                    }
                }
            };
            this.PushAsync(loadingScreen);

            var allContacts = DatabaseManager.GetRows<Contact>();
            var allContactsPage = new AllContactsPage(allContacts);
            SetHasBackButton(allContactsPage, false); // Prevent jumping back to the loading screen
            this.PushAsync(allContactsPage);
        }
    }
}