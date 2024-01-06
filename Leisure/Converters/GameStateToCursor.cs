using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using Leisure.Enums;

namespace Leisure.Converters
{
    public class GameStateToCursor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == (int)GameStates.Turn)
                return Cursors.None;
            else
                return Cursors.Arrow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
