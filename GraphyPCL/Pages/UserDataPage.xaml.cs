﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GraphyPCL
{
    public partial class UserDataPage : ContentPage
    {
        private UserDataViewModel _viewModel;

        public UserDataPage()
        {
            InitializeComponent();
            _tableView.Intent = TableIntent.Menu;

            _viewModel = new UserDataViewModel();
            BindingContext = _viewModel;
        }
    }
}