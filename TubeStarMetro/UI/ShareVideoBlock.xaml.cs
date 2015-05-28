using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace TubeStarMetro
{
    public sealed partial class ShareVideoBlock : UserControl
    {
        public ShareVideoBlock()
        {
            InitializeComponent();
            txtAdd.Text = EnglishStrings.AddVideo.Translate();
        }
    }
}