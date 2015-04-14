using System;
using Xamarin.Forms;

namespace GraphyPCL
{
    public class ArrowButtonBoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isToRelatedContact = (bool)value;
            if (isToRelatedContact)
            {
                return "=>";
            }
            else
            {
                return "<=";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = (string)value;
            if (text == "<=")
            {
                return false;
            }
            else
            {
                // Default to true even the text is not "=>"
                return true;
            }
        }
    }
}

