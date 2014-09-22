using System;
using GraphyPCL;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GraphyPCL.iOS;

[assembly: ExportRenderer(typeof(ExtendedTableView), typeof(ExtendedTableViewRenderer))]
namespace GraphyPCL.iOS
{
    public class ExtendedTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                ((ExtendedTableView)e.NewElement).DataChanged += (object sender, EventArgs args) => { Control.ReloadData(); };
            }
        }
    }
}

