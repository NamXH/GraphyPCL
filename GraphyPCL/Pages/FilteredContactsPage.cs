using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class FilteredContactsPage : AllContactsPage
    {
        public FilteredContactsPage()
            : base()
        {
            this.ToolbarItems.Clear();
            this.Title = "Search Result";
        }
    }
}