using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BackendClient.Model
{
    public class IntConvertToChartValues : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double srcValue = (double)value;
            IChartValues result = new ChartValues<ObservableValue>() { new ObservableValue(srcValue) };
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
