using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;

namespace GraphyPCL
{
    /// <summary>
    /// Picker GUID to int converter. Convert the selected index of the Picker to the Guid of the item and vice versa.
    /// Converter parameters: List<T> contains the items behind the picker, where T implement IIdContainer
    /// </summary>
    public class PickerGuidToIntConverter<T> : IValueConverter where T : IIdContainer
    {
        // From Guid to Index
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var guid = (Guid)value;
            var types = (List<T>)parameter;
            return types.FindIndex(x => x.Id == guid);
        }

        // From Index to Guid
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var index = (int)value;
            if (index == -1)
            {
                return Guid.Empty; // This condition is unreachable for some reason!! Bug!!
            }
            else
            {
                var types = (List<T>)parameter;
                return types[index].Id;
            }
        }
    }
}