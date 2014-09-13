using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class ContactDetailsPage : ContentPage
    {
        public ContactDetailsPage(Contact contact)
        {
            InitializeComponent();
            BindingContext = contact;
        }
    }
}

