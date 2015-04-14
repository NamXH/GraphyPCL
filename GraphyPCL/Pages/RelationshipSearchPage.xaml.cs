using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class RelationshipSearchPage : ContentPage
    {
        public RelationshipSearchPage()
        {
            InitializeComponent();
            _tableView.Intent = TableIntent.Menu;
        }
    }
}