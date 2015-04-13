using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class TagSearchPage : ContentPage
    {
        public IList<string> Criteria { get; set; }

        public TagSearchPage()
        {
            InitializeComponent();

            _tableView.Intent = TableIntent.Menu;

            Criteria = new List<string>();

            new AddMoreEntryCell(_tableView, _criteriaSection, Criteria);
        }
    }
}