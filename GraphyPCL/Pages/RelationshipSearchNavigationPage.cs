using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class RelationshipSearchNavigationPage : NavigationPage
    {
        public RelationshipSearchNavigationPage()
        {
            Icon = "genius_icon.png";
            Title = "Relation";
            this.PushAsync(new RelationshipSearchPage());
        }
    }
}