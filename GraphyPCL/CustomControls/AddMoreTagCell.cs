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
            this.ContainerTable.Navigation.PushAsync(new AddTagPage(ViewModel));
        }
    }
}