using System;
using Android.Widget;
using GraphyPCL;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GraphyPCL.Android;

[assembly: ExportRenderer(typeof(ExtendedTableView), typeof(ExtendedTableViewRenderer))]
namespace GraphyPCL.Android
{
    public class ExtendedTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                ((ExtendedTableView)e.NewElement).DataChanged += (object sender, EventArgs args) =>
                {
                    ((TableViewModelRenderer)Control.Adapter).NotifyDataSetChanged();
                };
            }
        }
    }
}