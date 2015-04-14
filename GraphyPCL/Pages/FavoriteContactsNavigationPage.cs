using System;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class FavoriteContactsNavigationPage : NavigationPage
    {
        public Page TopNavigationStack {
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
            var favoritePage = new ContactsPage(favoriteContacts);
            favoritePage.Title = "Favorite Contacts";

            this.PushAsync(favoritePage);
            this.Navigation.RemovePage(TopNavigationStack); // Hide the animation when popsync()
            TopNavigationStack = favoritePage;
        }
    }
}