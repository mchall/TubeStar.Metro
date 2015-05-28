using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace TubeStarMetro
{
    public enum MessagePicture
    {
        Axe,
        Error,
        Happy,
        Legal,
        Money,
        Puzzle,
        Question,
        Robot,
        Sad,
        Score,
        Static,
        Study,
        Work
    }

    public sealed partial class CustomMessageBox : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public bool Result { get; private set; }

        public CustomMessageBox(string title, string text, MessagePicture picture)
        {
            InitializeComponent();
            Translate();

            Caption.Text = title;
            messageBoxTextBlock.Text = text;
            imgMsg.Source = new BitmapImage(new Uri(String.Format("ms-appx:/Assets/Messages/{0}.jpg", picture)));

            btnCancel.HandPointerCursor();
            btnOk.HandPointerCursor();
            this.HandleDialogKeyboard(OKButton_Click, CancelButton_Click);
        }

        public void SetParent(PopupHelper parent)
        {
            _parent = parent;
        }

        public void OnFocus()
        {
        }

        private void Translate()
        {
            btnOk.Content = EnglishStrings.Ok.Translate();
            btnCancel.Content = EnglishStrings.Cancel.Translate();
        }

        public void HideCancelButton()
        {
            btnOk.Margin = btnCancel.Margin;
            btnCancel.Visibility = Visibility.Collapsed;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            _parent.Close(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            _parent.Close(false);
        }

        public static async void ShowDialog(string title, string text, MessagePicture picture, Action<bool?> onClose)
        {
            CustomMessageBox messageBox = new CustomMessageBox(title, text, picture);
            await messageBox.ShowAsync();
            if (onClose != null)
            {
                onClose(messageBox.Result);
            }
        }

        public static async void ShowDialog(string title, string text, MessagePicture picture)
        {
            CustomMessageBox messageBox = new CustomMessageBox(title, text, picture);
            messageBox.HideCancelButton();
            await messageBox.ShowAsync();
        }
    }
}