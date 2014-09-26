using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class ElementTypePage : ContentPage
    {
        public IList<string> LabelsList { get; set; }

        public ElementTypePage(IList<string> labelList)
        {
            InitializeComponent();
            LabelsList = labelList;
        }
    }
}

