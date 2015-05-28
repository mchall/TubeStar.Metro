using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TubeStarMetro
{
    public static class PopupExtension
    {
        public static async Task<bool?> ShowAsync(this UserControl dialog)
        {
            PopupHelper popup = new PopupHelper(dialog);
            await popup.ShowAsync();
            return popup.DialogResult;
        }
    }
}