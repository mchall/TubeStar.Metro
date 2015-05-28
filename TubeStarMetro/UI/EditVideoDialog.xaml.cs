using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace TubeStarMetro
{
    public sealed partial class EditVideoDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public Video Video { get; private set; }

        public EditVideoDialog(List<Video> videos)
        {
            InitializeComponent();
            Translate();
            //FocusedElement = cmbVideo;

            cmbVideo.ItemsSource = GetData(videos);
            cmbVideo.DisplayMemberPath = "Value";
            cmbVideo.SelectedValuePath = "Key";

            if (videos.Count == 1)
            {
                cmbVideo.SelectedValue = videos[0];
                cmbVideo.IsEnabled = false;
            }

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
            cmbVideo.Focus(FocusState.Programmatic);
        }

        private void Translate()
        {
            //Caption = EnglishStrings.EditVideo.Translate();
            lblVideo.Text = EnglishStrings.Video.Translate() + ":";
            lblHoursSelect.Text = EnglishStrings.Hours.Translate() + ":";
            btnOk.Content = EnglishStrings.Ok.Translate();
            btnCancel.Content = EnglishStrings.Cancel.Translate();
            lblHours.Text = "4 " + EnglishStrings.Hours.Translate().ToLower();
        }

        private Dictionary<Video, string> GetData(List<Video> videos)
        {
            Dictionary<Video, string> data = new Dictionary<Video, string>();
            foreach (var video in videos)
            {
                data[video] = video.Name;
            }
            return data;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (cmbVideo.SelectedValue == null)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.MissingValueHeader.Translate(), EnglishStrings.MissingVideo.Translate(), MessagePicture.Puzzle);
                return;
            }

            Video = (Video)cmbVideo.SelectedValue;
            Video.ExtraEditingHours = (int)sldrHours.Value - EditVideo.MinimumEditTime;
            _parent.Close(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(false);
        }

        private void sldrHours_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (lblHours != null)
                lblHours.Text = String.Format("{0} {1}", (int)sldrHours.Value, EnglishStrings.Hours.Translate().ToLower());
        }
    }
}