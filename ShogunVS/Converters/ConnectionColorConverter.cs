using ShogunVS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ShogunVS.Converters
{
    public class ConnectionColorConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is ConnectionStatus))
                return Brushes.Black;

            ConnectionStatus connectionStatus = (ConnectionStatus)value;
            switch (connectionStatus)
            {
                case ConnectionStatus.Disconnected:
                    return Brushes.DarkRed;
                case ConnectionStatus.Standby:
                    return Brushes.DimGray;
                case ConnectionStatus.Online:
                    return Brushes.DarkOliveGreen;
                case ConnectionStatus.Connected:
                    return Brushes.DarkOliveGreen;
                default:
                    return Brushes.Gray;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
  
