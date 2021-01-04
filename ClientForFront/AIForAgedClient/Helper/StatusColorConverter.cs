using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AIForAgedClient
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool tempValue = (bool)value;
            if (tempValue)
            {
                return new SolidColorBrush(Colors.Blue);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}