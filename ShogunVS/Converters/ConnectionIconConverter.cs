﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ShogunVS.Models;
using System.Windows.Markup;

namespace ShogunVS.Converters
{
    public class ConnectionIconConverter : MarkupExtension, IValueConverter
    {
        public string StatusType { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null || !(value is ConnectionStatus))
                return PackIconKind.LanDisconnect;
            ConnectionStatus connectionStatus = (ConnectionStatus)value;
            if (StatusType == "Camera")
            {
                switch (connectionStatus)
                {
                    case ConnectionStatus.Disconnected:
                        return PackIconKind.CameraDocumentOff;
                    case ConnectionStatus.Connected:
                        return PackIconKind.CameraDocument;
                    default:
                        return PackIconKind.CameraDocumentOff;
                }
            }
            return PackIconKind.CameraDocumentOff;
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
