using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace VintageGarmentDescriber
{
    public class ShortcutButtonChooserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MainWindow wnd = (MainWindow) value;
            if (wnd == null)
                return null;

            Grid group = wnd.GetVisibleGarmentUIGroup();
            Button btn = Utils.GetButtonByNamePart(group, parameter.ToString());
            return btn == null ? wnd.Dress : btn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }



    }

    public class PassThroughConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.ToList();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
