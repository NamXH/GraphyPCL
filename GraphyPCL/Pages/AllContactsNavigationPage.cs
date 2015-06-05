using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Plugin;
using Xamarin.Forms;
using Plugin = Contacts.Plugin.Abstractions;
using System.Linq;

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

            CopyExistingContactsAndCreateAllContactsPage();
        }

        private async void CopyExistingContactsAndCreateAllContactsPage()
        {
            if (await CrossContacts.Current.RequestPermission())
            {
                List<Plugin.Contact> builtInContacts = null;
                CrossContacts.Current.PreferContactAggregation = false; //recommended by author

                // Maybe have to use Task to run in background
                //                await Task.Run(() =>
                //                    {
                //                    };

                if (CrossContacts.Current.Contacts == null)
                {
                    return;
                }

                builtInContacts = CrossContacts.Current.Contacts.ToList();

                foreach (var builtInContact in builtInContacts)
                {
                    var contact = new Contact();
                    contact.Id = Guid.NewGuid();
                    contact.FirstName = builtInContact.FirstName;
                    contact.MiddleName = builtInContact.MiddleName;
                    contact.LastName = builtInContact.LastName;

                    var organization = builtInContact.Organizations.FirstOrDefault();
                    if (organization != null)
                    {
                        contact.Organization = organization.Name;
                    }

                    DatabaseManager.DbConnection.Insert(contact);

                    foreach (var builtInAddress in builtInContact.Addresses)
                    {
                        var address = new Address();
                        address.Id = Guid.NewGuid();
                        address.Type = builtInAddress.Type.ToString();
                        address.StreetLine1 = builtInAddress.StreetAddress;
                        address.City = builtInAddress.City;
                        address.Province = builtInAddress.Region;
                        address.Country = builtInAddress.Country;
                        address.PostalCode = builtInAddress.PostalCode;

                        address.ContactId = contact.Id;
                        DatabaseManager.DbConnection.Insert(address);
                    }

                    foreach (var builtInIM in builtInContact.InstantMessagingAccounts)
                    {
                        var im = new InstantMessage();
                        im.Id = Guid.NewGuid();
                        im.Type = builtInIM.Service.ToString();
                        im.Nickname = builtInIM.Account;

                        im.ContactId = contact.Id;
                        DatabaseManager.DbConnection.Insert(im);
                    }

                    foreach (var builtInWebsite in builtInContact.Websites)
                    {
                        var url = new Url();
                        url.Id = Guid.NewGuid();
                        url.Link = builtInWebsite.Address;

                        url.ContactId = contact.Id;
                        DatabaseManager.DbConnection.Insert(url);
                    }

                    // We consider existing notes as tags
                    foreach (var builtInNote in builtInContact.Notes)
                    {
                        var tag = new Tag();
                        tag.Id = Guid.NewGuid();
                        tag.Name = builtInNote.Contents;

                        DatabaseManager.DbConnection.Insert(tag);

                        var tagMap = new ContactTagMap
                        {
                            Id = Guid.NewGuid(),
                            ContactId = contact.Id,
                            TagId = tag.Id,
                        };
                        DatabaseManager.DbConnection.Insert(tagMap);
                    }

                    foreach (var builtInEmail in builtInContact.Emails)
                    {
                        var email = new Email();
                        email.Id = Guid.NewGuid();
                        email.Type = builtInEmail.Type.ToString();
                        email.Address = builtInEmail.Address;

                        email.ContactId = contact.Id;
                        DatabaseManager.DbConnection.Insert(email);
                    }

                    foreach (var builtInPhone in builtInContact.Phones)
                    {
                        var phoneNumber = new PhoneNumber();
                        phoneNumber.Id = Guid.NewGuid();
                        phoneNumber.Type = builtInPhone.Type.ToString();
                        phoneNumber.Number = builtInPhone.Number;

                        phoneNumber.ContactId = contact.Id;
                        DatabaseManager.DbConnection.Insert(phoneNumber);
                    }
                }
            }

            var allContacts = DatabaseManager.GetRows<Contact>();
            var allContactsPage = new AllContactsPage(allContacts);
            SetHasBackButton(allContactsPage, false); // Prevent jumping back to the loading screen
            this.PushAsync(allContactsPage);
        }
    }
}