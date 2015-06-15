using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.SelectedItem = _allContactsNavigationPage;

            // Not showing Favourite Page to workaround bug: System.ArgumentException: 'jobject' must not be IntPtr.Zero
            Device.OnPlatform(Android: () => this.Children.RemoveAt(0));
        }
    }
}