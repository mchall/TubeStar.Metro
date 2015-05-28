using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace TubeStarMetro
{
    public sealed partial class AddVideoDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;
        private List<VideoAttribute> _currentAttributes;
        private bool _ignoreSelectEvent;

        public Video Video { get; private set; }

        public AddVideoDialog()
        {
            InitializeComponent();
            Translate();

            _currentAttributes = new List<VideoAttribute>();

            var data = GetData();
            cmbCategory.ItemsSource = data;
            cmbCategory.DisplayMemberPath = "Value";
            cmbCategory.SelectedValuePath = "Key";

            if (Settings.LastCategory.HasValue)
            {
                cmbCategory.SelectedIndex = data.IndexOf(Settings.LastCategory.Value);
            }

            PopulateAttributeButtons();
            UpdateAttributePointLabel(Player.Current.VideoAttributePoints);

            UpdateButtons();

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
            txtName.Focus(FocusState.Programmatic);
        }

        private void Translate()
        {
            //Caption.Text = EnglishStrings.AddVideo.Translate();
            lblName.Text = EnglishStrings.Name.Translate() + ":";
            lblCategory.Text = EnglishStrings.Category.Translate() + ":";
            lblHourSelect.Text = EnglishStrings.Hours.Translate() + ":";
            lblCost.Text = EnglishStrings.Cost.Translate() + ":";
            lblHours.Text = "2 " + EnglishStrings.Hours.Translate().ToLower();
            btnOk.Content = EnglishStrings.Next.Translate();
            btnCancel.Content = EnglishStrings.Cancel.Translate();
        }

        private void PopulateAttributeButtons()
        {
            btnCats.VideoAttribute = VideoAttributes.Cats;
            btnCopycat.VideoAttribute = VideoAttributes.CopyCat;
            btnFanBoyBat.VideoAttribute = VideoAttributes.FanBoyBait;
            btnGenreBuster.VideoAttribute = VideoAttributes.GenreBuster;
            btnHypnotic.VideoAttribute = VideoAttributes.Hypnotic;
            btnLearnFromMistakes.VideoAttribute = VideoAttributes.LearnFromMistakes;
            btnMemetic.VideoAttribute = VideoAttributes.Memetic;
            btnProductPlacement.VideoAttribute = VideoAttributes.ProductPlacement;
            btnSecondTime.VideoAttribute = VideoAttributes.SecondTime;
            btnSoBad.VideoAttribute = VideoAttributes.SoBad;
            btnCrowdfunding.VideoAttribute = VideoAttributes.Crowdfunding;
            btnAboveBoard.VideoAttribute = VideoAttributes.AboveBoard;
        }

        private void UpdateAttributePointLabel(int count)
        {
            lblAttributeSelect.Text = String.Format("{0}: ({1} {2})", EnglishStrings.Attributes.Translate(), count, EnglishStrings.PointsLeft.Translate());
        }

        private void SelectButton_SelectedChanged(object sender, EventArgs e)
        {
            if (_ignoreSelectEvent)
                return;

            var selectButton = sender as SelectButton;
            if (selectButton != null)
            {
                if (selectButton.Selected)
                    _currentAttributes.Add(selectButton.VideoAttribute);
                else
                    _currentAttributes.Remove(selectButton.VideoAttribute);

                int totalAttributePoints = 0;
                foreach (var attrib in _currentAttributes)
                {
                    totalAttributePoints += attrib.Cost;
                }

                if (totalAttributePoints > Player.Current.VideoAttributePoints)
                {
                    _ignoreSelectEvent = true;
                    selectButton.Selected = false;
                    _currentAttributes.Remove(selectButton.VideoAttribute);
                    _ignoreSelectEvent = false;
                }
                else
                {
                    UpdateAttributePointLabel(Player.Current.VideoAttributePoints - totalAttributePoints);
                }

                UpdateButtons();
            }
        }

        private void UpdateButtons()
        {
            int totalCost = 0;
            foreach (var attrib in _currentAttributes)
            {
                totalCost += attrib.Cost;
            }

            foreach (SelectButton button in attributeGrid.Children)
            {
                if (!button.Selected && button.VideoAttribute.Cost + totalCost > Player.Current.VideoAttributePoints)
                {
                    button.IsEnabled = false;
                }
                else
                {
                    button.IsEnabled = true;
                }
                button.HandPointerCursor();
            }
        }

        private Dictionary<VideoCategory, string> GetData()
        {
            Dictionary<VideoCategory, string> data = new Dictionary<VideoCategory, string>();
            foreach (VideoCategory category in Enum.GetValues(typeof(VideoCategory)))
            {
                data[category] = category.GetString();
            }

            List<KeyValuePair<VideoCategory, string>> sortTemp = data.ToList();
            sortTemp.Sort((l, r) => l.Value.CompareTo(r.Value));

            return sortTemp.ToDictionary((s) => s.Key, (s) => s.Value);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                CustomMessageBox.ShowDialog(EnglishStrings.MissingValueHeader.Translate(), EnglishStrings.MissingName.Translate(), MessagePicture.Puzzle);
                return;
            }

            if (cmbCategory.SelectedValue == null)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.MissingValueHeader.Translate(), EnglishStrings.MissingCategory.Translate(), MessagePicture.Puzzle);
                return;
            }

            if (Player.Current.Money - (sldrMoney.Value * 100) < 0)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.LowCashHeader.Translate(), EnglishStrings.LowCashMessage.Translate(), MessagePicture.Money);
                return;
            }

            if (step2.Visibility == Visibility.Collapsed)
            {
                step1.Visibility = Visibility.Collapsed;
                step2.Visibility = Visibility.Visible;
                btnOk.Content = EnglishStrings.Ok.Translate();
                return;
            }

            Video = new Video();
            Video.Name = txtName.Text;
            Video.Category = (VideoCategory)cmbCategory.SelectedValue;
            Video.ExtraShootingHours = (int)sldrHours.Value - ShootVideo.MinimumShootTime;
            Video.Cost = (int)sldrMoney.Value * 100;
            Video.Attributes = _currentAttributes;
            Video.FetchRandomImage();

            Settings.LastCategory = Video.Category;
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

        private void sldrMoney_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (lblMoney != null)
                lblMoney.Text = String.Format("${0}", (int)sldrMoney.Value * 100);
        }
    }
}