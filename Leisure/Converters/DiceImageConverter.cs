using System;
using System.Globalization;
using System.Windows.Data;

namespace Leisure.Converters
{
    public class DiceImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Format("/Images/Dice{0}.png", (int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
