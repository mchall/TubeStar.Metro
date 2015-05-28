using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TubeStarMetro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TitlePage _titlePage;
        private GameMain _mainPage;
        private CommunityControl _community;

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += WindowSizeChanged;
            Initiate();
        }

        private async void Initiate()
        {
            if (CultureInfo.CurrentCulture.Name.StartsWith("ru-"))
            {
                await Languages.SetLanguage(TubeStarMetro.Language.Russian);
            }
            else if (CultureInfo.CurrentCulture.Name.StartsWith("pl-"))
            {
                await Languages.SetLanguage(TubeStarMetro.Language.Polish);
            }
            else if (CultureInfo.CurrentCulture.Name.StartsWith("de-"))
            {
                await Languages.SetLanguage(TubeStarMetro.Language.German);
            }
            else if (CultureInfo.CurrentCulture.Name.StartsWith("sv-"))
            {
                await Languages.SetLanguage(TubeStarMetro.Language.Swedish);
            }
            else if (CultureInfo.CurrentCulture.Name.StartsWith("fr-"))
            {
                await Languages.SetLanguage(TubeStarMetro.Language.French);
            }

            _titlePage = new TitlePage();
            _titlePage.NewGameClicked += TitlePage_NewGameClicked;
            _titlePage.ContinueGameClicked += TitlePage_ContinueGameClicked;
            _titlePage.CommunityClicked += TitlePage_CommunityClicked;
            SetContent(_titlePage);
        }

        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            SnappedGrid.Visibility = (ApplicationView.Value == ApplicationViewState.Snapped) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetContent(UserControl page)
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(page);
        }

        private void TitlePage_NewGameClicked(bool careerMode, bool payed)
        {
            VideoViewer.Reset();
            if (!payed)
                Player.Current.Reset();
            else
                Player.Current.BornToRule();
            Player.Current.ChallengeMode = careerMode;

            _mainPage = new GameMain();
            _mainPage.GameLose += MainPage_GameLose;
            _mainPage.GameExit += MainPage_GameExit;
            _mainPage.GameDeath += MainPage_GameDeath;
            SetContent(_mainPage);
        }

        private void TitlePage_CommunityClicked()
        {
            if (_community == null)
            {
                _community = new CommunityControl();
                _community.BackClicked += Community_BackClicked;
            }
            SetContent(_community);
        }

        private void Community_BackClicked()
        {
            SetContent(_titlePage);
            _titlePage.Refresh();
        }

        private async void TitlePage_ContinueGameClicked()
        {
            VideoViewer.Reset();
            Player.Current.Reset();
            this.IsEnabled = false;
            await SaveLoadHelper.Load(SaveLoadHelper.SaveFile);
            this.IsEnabled = true;

            _mainPage = new GameMain();
            _mainPage.GameLose += MainPage_GameLose;
            _mainPage.GameExit += MainPage_GameExit;
            _mainPage.GameDeath += MainPage_GameDeath;
            SetContent(_mainPage);
        }

        private void MainPage_GameDeath()
        {
            SetContent(_titlePage);
            _titlePage.Refresh();
            CustomMessageBox.ShowDialog(EnglishStrings.Death.Translate(), EnglishStrings.RobotDeath.Translate(), MessagePicture.Robot);
        }

        private void MainPage_GameExit()
        {
            SetContent(_titlePage);
            _titlePage.Refresh();
        }

        private void MainPage_GameLose()
        {
            SetContent(_titlePage);
            _titlePage.Refresh();
            CustomMessageBox.ShowDialog(EnglishStrings.WhatALoser.Translate(), EnglishStrings.OutOfMoney.Translate(), MessagePicture.Money);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}