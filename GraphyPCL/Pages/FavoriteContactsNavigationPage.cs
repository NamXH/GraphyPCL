using System;
using System.Linq;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class FavoriteContactsNavigationPage : NavigationPage
    {
        public FavoriteContactsNavigationPage()
        {
            Icon = "star_icon.png";
            Title = "Favorite";

            var favoriteContacts = DatabaseManager.GetRows<Contact>().Where(x => x.Favorite == true).ToList();
            var favoritePage = new ContactsPage(favoriteContacts);
            favoritePage.Title = "Favorite Contacts";
            this.PushAsync(favoritePage);
        }
    }
}