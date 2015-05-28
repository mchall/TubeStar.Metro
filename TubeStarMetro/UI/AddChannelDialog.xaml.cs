using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TubeStarMetro
{
    public sealed partial class AddChannelDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public Channel Channel { get; private set; }

        public AddChannelDialog()
        {
            InitializeComponent();
            Translate();

            var data = GetData();
            cmbAdvertising.ItemsSource = data;
            cmbAdvertising.DisplayMemberPath = "Value";
            cmbAdvertising.SelectedValuePath = "Key";

            cmbAdvertising.SelectedIndex = data.IndexOf(AdvertisingStrategy.Normal);

            btnOk.HandPointerCursor();
            btnCancel.HandPointerCursor();
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
            //Caption = EnglishStrings.AddChannel.Translate();
            lblName.Text = EnglishStrings.Name.Translate() + ":";
            lblStrategy.Text = EnglishStrings.AdvertisingStrategy.Translate() + ":";
            btnOk.Content = EnglishStrings.Ok.Translate();
            btnCancel.Content = EnglishStrings.Cancel.Translate();
        }

        public AddChannelDialog(Channel channel)
            : this()
        {
            //Caption = EnglishStrings.EditChannel.Translate();
            Channel = channel;
            txtName.Text = channel.Name;

            var data = GetData();
            cmbAdvertising.SelectedIndex = data.IndexOf(channel.Advertising);
        }

        private Dictionary<AdvertisingStrategy, string> GetData()
        {
            Dictionary<AdvertisingStrategy, string> data = new Dictionary<AdvertisingStrategy, string>();
            data[AdvertisingStrategy.Low] = GetEnumString(AdvertisingStrategy.Low);
            data[AdvertisingStrategy.Normal] = GetEnumString(AdvertisingStrategy.Normal);
            data[AdvertisingStrategy.High] = GetEnumString(AdvertisingStrategy.High);
            data[AdvertisingStrategy.Aggressive] = GetEnumString(AdvertisingStrategy.Aggressive);
            return data;
        }

        private string GetEnumString(AdvertisingStrategy strategy)
        {
            var income = strategy.GetAttribute<AdvertistingIncomeAttribute>().IncomePerView;
            return String.Format("{0} ({1} {2})", strategy.GetString(),
                Player.Current.ChallengeMode ? (income / 2).ToCurrencyString() : income.ToCurrencyString(),
                EnglishStrings.PerView.Translate());
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                CustomMessageBox.ShowDialog(EnglishStrings.MissingValueHeader.Translate(), EnglishStrings.MissingName.Translate(), MessagePicture.Puzzle);
                return;
            }

            if (cmbAdvertising.SelectedValue == null)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.MissingValueHeader.Translate(), EnglishStrings.MissingStrategy.Translate(), MessagePicture.Puzzle);
                return;
            }

            Channel = new Channel()
            {
                Name = txtName.Text,
                Advertising = (AdvertisingStrategy)cmbAdvertising.SelectedValue,
            };
            _parent.Close(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(false);
        }
    }
}