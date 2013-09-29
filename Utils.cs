using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VintageGarmentDescriber
{
    class Utils
    {
        static public DependencyObject GetTopLevelControl(DependencyObject control)
        {
            DependencyObject tmp = control;
            DependencyObject parent = null;
            while ((tmp = LogicalTreeHelper.GetParent(tmp)) != null)
            {
                parent = tmp;
            }
            return parent;
        }

        static public Button GetButtonByNamePart(Visual control, String str)
        {
            if (control == null)
                return null;

            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            Button btn = null;
            for (int i = 0; i <= childNumber - 1; i++)
            {
                DependencyObject obj = VisualTreeHelper.GetChild(control, i);
                if (obj is Button)
                {
                    btn = (Button)obj;
                    int pos = btn.Content.ToString().LastIndexOf(' ');
                    String str02 = btn.Content.ToString().Substring(pos, 2);
                    if (str02 != null && str.Trim().Equals(str.Trim(), StringComparison.InvariantCultureIgnoreCase))
                        return btn;
                }
            }
            return null;
        }
    }
}
