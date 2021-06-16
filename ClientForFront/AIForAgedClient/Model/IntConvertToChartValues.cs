using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIForAgedClient.Model
{
    public class IntConvertToChartValues : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int srcValue = (int)value;
            IChartValues result = new ChartValues<ObservableValue>() { new ObservableValue(srcValue) };
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}