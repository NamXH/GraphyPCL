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

            // Workaround for a bug!!
            // On EditContactPage, if user pushes Back button, everything will be save instead of ignored
            this.Popped += (object sender, NavigationEventArgs e) => 
                {
                    if (e.Page.GetType() == typeof(AddEditContactPage))
                    {
                        var page = (AddEditContactPage)e.Page;
                        var contact = page.ViewModel.Contact;
                        MessagingCenter.Send<AllContactsNavigationPage, Contact>(this, "Update", contact);

                        Navigation.PopToRootAsync(); // Since the DetailContactPage is not updated, we don't want to show it.
                    }
                };
        }
    }
}