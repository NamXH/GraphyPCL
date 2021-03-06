﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
    /// <summary>
    /// Picker: string to int converter. Convert the selected index of the Picker to the string of the item and vice versa.
    /// Converter parameters: List<string> contains the items in the picker.
    /// </summary>
    public class PickerStringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var itemToFind = (string)value;
            var itemList = (List<string>)parameter;
            return itemList.FindIndex(x => x == itemToFind);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var index = (int)value;
            if (index == -1)
            {
                return null;
            }
            else
            {
                var itemList = (List<string>)parameter;
                return itemList[index];
            }
        }
    }
}