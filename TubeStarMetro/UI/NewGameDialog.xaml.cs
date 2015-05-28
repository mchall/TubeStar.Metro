using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace TubeStarMetro
{
    public sealed partial class NewGameDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public event Action CasualClicked;
        public event Action CareerClicked;
        public event Action BornToRuleClicked;

        public NewGameDialog()
        {
            this.InitializeComponent();
            Translate();

            btnCancel.HandPointerCursor();
            btnCareer.HandPointerCursor();
            btnCasual.HandPointerCursor();
            this.HandleDialogKeyboard(null, CancelButton_Click);
        }

        private void Translate()
        {
            //Caption.Text = EnglishStrings.NewGame.Translate();
            txtCasual.Text = EnglishStrings.Casual.Translate();
            txtChallenge.Text = EnglishStrings.Challenge.Translate();
            txtBornToRule.Text = EnglishStrings.BornToRule.Translate();
            btnCancel.Content = EnglishStrings.Cancel.Translate();
        }

        private void Casual_Click(object sender, RoutedEventArgs e)
        {
            if (CasualClicked != null)
            {
                CasualClicked();
                _parent.Close(null);
            }
        }

        private void Career_Click(object sender, RoutedEventArgs e)
        {
            if (CareerClicked != null)
            {
                CareerClicked();
                _parent.Close(null);
            }
        }

        private async void BornToRule_Click(object sender, RoutedEventArgs e)
        {
            if (!InAppPurchaseHelper.HasPurchase(InAppPurchase.BornToRule))
            {
                _parent.Close(null);
                await InAppPurchaseHelper.BuyFeature(InAppPurchase.BornToRule);
            }
            else
            {
                if (BornToRuleClicked != null)
                {
                    BornToRuleClicked();
                    _parent.Close(null);
                }
            }
        }

        public void SetParent(PopupHelper parent)
        {
            _parent = parent;
        }

        public void OnFocus()
        {
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(false);
        }
    }
}