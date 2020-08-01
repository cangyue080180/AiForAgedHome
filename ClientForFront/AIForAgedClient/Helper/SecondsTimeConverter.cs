using System;
using System.Globalization;
using System.Windows.Data;

namespace AIForAgedClient
{
    internal class SecondsTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int seconds = (int)value;
            TimeSpan timeSpan = new TimeSpan(0, 0, seconds);
            return timeSpan.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
