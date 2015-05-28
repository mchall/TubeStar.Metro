using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace TubeStarMetro
{
    public sealed partial class TitlePage : UserControl
    {
        public event Action ContinueGameClicked;
        public event Action<bool, bool> NewGameClicked;
        public event Action CommunityClicked;

        private List<string> _tips;

        public TitlePage()
        {
            this.InitializeComponent();
            Refresh();

            btnContinue.HandPointerCursor();
            btnCredits.HandPointerCursor();
            btnNewGame.HandPointerCursor();
            btnTutorial.HandPointerCursor();
            translatorLink.HandPointerCursor();
        }

        private void ContinueGame_Click(object sender, RoutedEventArgs e)
        {
            if (ContinueGameClicked != null)
                ContinueGameClicked();
        }

        private async void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGameDialog dialog = new NewGameDialog();
            dialog.CasualClicked += () =>
            {
                if (NewGameClicked != null)
                    NewGameClicked(false, false);
            };
            dialog.CareerClicked += () =>
            {
                if (NewGameClicked != null)
                    NewGameClicked(true, false);
            };
            dialog.BornToRuleClicked += () =>
            {
                if (NewGameClicked != null)
                    NewGameClicked(false, true);
            };

            await dialog.ShowAsync();
        }

        private async void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.youtube.com/watch?v=oKfNSm1SLSQ"));
        }

        private void Community_Click(object sender, RoutedEventArgs e)
        {
            if (CommunityClicked != null)
                CommunityClicked();
        }

        private async void Credits_Click(object sender, RoutedEventArgs e)
        {
            CreditsDialog dialog = new CreditsDialog();
            await dialog.ShowAsync();
        }

        private async void RemoveAds_Click(object sender, RoutedEventArgs e)
        {
            await InAppPurchaseHelper.BuyFeature(InAppPurchase.RemoveAds);
            Refresh();
        }

        private async void BornToRule_Click(object sender, RoutedEventArgs e)
        {
            await InAppPurchaseHelper.BuyFeature(InAppPurchase.BornToRule);
            Refresh();
        }

        public async void Refresh()
        {
            _tips = new List<string>();
            _tips.Add(EnglishStrings.TitlePageTip1.Translate());
            _tips.Add(EnglishStrings.TitlePageTip2.Translate());
            _tips.Add(EnglishStrings.TitlePageTip3.Translate());
            _tips.Add(EnglishStrings.TitlePageTip4.Translate());
            _tips.Add(EnglishStrings.TitlePageTip5.Translate());
            _tips.Add(EnglishStrings.TitlePageTip6.Translate());
            _tips.Add(EnglishStrings.TitlePageTip7.Translate());
            _tips.Add(EnglishStrings.TitlePageTip8.Translate());

            txtTip.Text = String.Format(EnglishStrings.Tip.Translate() + ": {0}", _tips[RandomHelpers.RandomInt(_tips.Count)]);
            btnContinue.IsEnabled = await SaveLoadHelper.SaveExistsAsync();

            Translate();
        }

        private void Translate()
        {
            btnContinue.Content = EnglishStrings.ContinueGame.Translate();
            btnNewGame.Content = EnglishStrings.NewGame.Translate();
            btnTutorial.Content = EnglishStrings.Tutorial.Translate();
            btnCredits.Content = EnglishStrings.Credits.Translate();
            btnCommunity.Content = EnglishStrings.Community.Translate();

            translatorLink.Visibility = Visibility.Collapsed;
            translator.Visibility = Visibility.Visible;
            switch (Languages.CurrentLanguage)
            {
                case TubeStarMetro.Language.German:
                    translator.Text = "iKlikla";
                    break;
                case TubeStarMetro.Language.Polish:
                    translator.Text = "Norbertgrom & Kaspartos";
                    break;
                case TubeStarMetro.Language.Russian:
                    translator.Text = "Lolycon & SamaraGamer";
                    break;
                case TubeStarMetro.Language.Swedish:
                    translator.Text = "Swertayy";
                    break;
                case TubeStarMetro.Language.French:
                    translatorLink.Visibility = Visibility.Visible;
                    translator.Visibility = Visibility.Collapsed;
                    translatorLinkText.Text = "SoldierX3";
                    break;
                default:
                    translator.Text = String.Empty;
                    break;
            }
        }

        private async void Language_ItemClick(object sender, ItemClickEventArgs e)
        {
            var image = e.ClickedItem as Image;
            if (image != null)
            {
                int result;
                if (image.Tag != null && int.TryParse(image.Tag.ToString(), out result))
                {
                    await Languages.SetLanguage((TubeStarMetro.Language)result);
                    Refresh();
                }
                else
                {
                    await Launcher.LaunchUriAsync(new Uri("http://tubestar.azurewebsites.net/Translation"));
                }
            }
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Languages.CurrentLanguage)
            {
                case TubeStarMetro.Language.French:
                    await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/soldierx3softwares"));
                    break;
            }
        }
    }
}