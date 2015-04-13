using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class FilteredContactsPage : ContactsPage
    {
        public FilteredContactsPage(IList<Contact> contacts)
            : base(contacts)
        {
            this.Title = "Search Result";
        }
    }
}