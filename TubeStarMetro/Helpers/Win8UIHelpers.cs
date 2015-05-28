using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace TubeStarMetro
{
    public static class Win8UIHelpers
    {
        public static void ToggleClicked(this ToggleButton button)
        {
            VisualStateManager.GoToState(button, button.IsChecked.Value ? "Checked" : "Unchecked", false);
        }

        public static void HandleDialogKeyboard(this UserControl dialog, RoutedEventHandler onEnter, RoutedEventHandler onCancel)
        {
            dialog.KeyDown += (s, e) =>
                {
                    if (e.Key == Windows.System.VirtualKey.Enter)
                    {
                        if (onEnter != null)
                        {
                            onEnter(null, new RoutedEventArgs());
                            e.Handled = true;
                        }
                    }
                    else if (e.Key == Windows.System.VirtualKey.Escape)
                    {
                        if (onCancel != null)
                        {
                            onCancel(null, new RoutedEventArgs());
                            e.Handled = true;
                        }
                    }
                };
        }

        public static void HandPointerCursor(this UIElement element)
        {
            element.PointerEntered += (s, e) => Window.Current.CoreWindow.PointerCursor = new CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            element.PointerExited += (s, e) => Window.Current.CoreWindow.PointerCursor = new CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
        }
    }
}