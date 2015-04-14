using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AllContactsNavigationPage : NavigationPage
    {
        public AllContactsNavigationPage()
        {
            Icon = "contact_icon.png";
            Title = "Contacts";
            var allContacts = DatabaseManager.GetRows<Contact>();
            this.PushAsync(new AllContactsPage(allContacts));
        }
    }
}