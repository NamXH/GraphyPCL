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
        public void Refresh()
        {
            Control.ReloadData();
        }
    }
}

