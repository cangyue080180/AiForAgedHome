using DataModel;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIForAgedClient
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            byte num = (byte)value;
            PoseStateTypes type = (PoseStateTypes)num;
            switch (type)
            {
                case PoseStateTypes.Down:
                    return "地上";
                case PoseStateTypes.Lie:
                    return "躺";
                case PoseStateTypes.Sit:
                    return "坐";
                case PoseStateTypes.Stand:
                    return "站";
                case PoseStateTypes.Other:
                    return "其他";
                default:
                    return "";
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
