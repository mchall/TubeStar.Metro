using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TubeStarMetro
{
    public class CustomGridView : GridView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var block = item as VideoBlock;
            if (block != null)
            {
                if (block.Featured)
                {
                    element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 2);
                    element.SetValue(VariableSizedWrapGrid.RowSpanProperty, 2);
                }
            }
            base.PrepareContainerForItemOverride(element, item);
        }
    }
}