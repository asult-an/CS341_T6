using System;
using System.Globalization;
using System.IO;
using Microsoft.Maui.Controls;

namespace CookNook.XMLHelpers;

public class ByteToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte[] byteArray && byteArray.Length > 0)
        {
            return ImageSource.FromStream(() => new MemoryStream(byteArray));
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
