using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GraphyPCL
{
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