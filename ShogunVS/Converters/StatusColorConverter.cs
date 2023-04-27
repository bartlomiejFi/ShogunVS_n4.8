using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace ShogunVS.Converters
{
    public class StatusColorConverter : MarkupExtension, IValueConverter
    {

        public string StatusType { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool))
                return Brushes.Gray;


            if ((bool)value)
            {
                if(StatusType == "Bar")
                    return Brushes.Green;
                else
                    return Brushes.CornflowerBlue;
            }
            else
            {
                if (StatusType == "Bar")
                    return Brushes.DarkRed;
                else
                    return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
