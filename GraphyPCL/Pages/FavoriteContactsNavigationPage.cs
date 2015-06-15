using System;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class FavoriteContactsNavigationPage : NavigationPage
    {
        public Page TopNavigationStack
        {
            get;
            set;
        }

        public FavoriteContactsNavigationPage()
        {
            Icon = "star_icon.png";
            Title = "Favorite";

            TopNavigationStack = new Page();
            this.PushAsync(TopNavigationStack);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var favoriteContacts = DatabaseManager.GetRows<Contact>().Where(x => x.Favorite == true).ToList();
            var favoritePage = new ContactsPage(favoriteContacts, false)
            {
                Title = "Favorite Contacts"
            };

            Device.OnPlatform(
                iOS: () => // Special implementation for better UX on iOS, does not work on Android
                {
                    // Everytime, push a new page with updated favorite contacts to the navigation stack then remove the old one
                    this.PushAsync(favoritePage);
                    this.Navigation.RemovePage(TopNavigationStack); // Do this to hide the animation when popsync()
                    TopNavigationStack = favoritePage;
                },
                Default: () => // Simple implementation
                {
                    Navigation.PopAsync();
                    Navigation.PushAsync(favoritePage);
                }
            );
        }
    }
}