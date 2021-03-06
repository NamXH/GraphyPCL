﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GraphyPCL
{
    /// <summary>
    /// Contacts grouped by a title (first character of contact's fullname).
    /// Use this class when need a title for the group.
    /// The class should extend IEnumerable so a ListView in Xamarin.Forms can use it.
    /// Note: The enumerator of the class is actually the enumerator of the List<Contact> property.
    /// </summary>
    public class ContactsGroup : ObservableCollection<Contact>
    {
        public string Title { get; private set; }

        public ContactsGroup(string title)
        {
            Title = title;
        }
    }
}