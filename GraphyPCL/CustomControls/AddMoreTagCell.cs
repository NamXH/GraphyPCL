using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class AddMoreTagCell : AddMoreElementCell
    {
        public ContactViewModel ViewModel { get; set; }

        public AddMoreTagCell(ExtendedTableView table, TableSection tableSection, ContactViewModel viewModel)
            : base(table, tableSection)
        {
            ViewModel = viewModel;
        }

        protected override void OnCellClicked(object sender, EventArgs args)
        {
            var viewCell = new ViewCell();
            ContainerSection.Insert(ContainerSection.Count - 1, viewCell);



            ContainerTable.OnDataChanged();
        }
    }
}