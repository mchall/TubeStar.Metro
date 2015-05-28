using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TubeStarMetro
{
    public sealed partial class GameMain : UserControl
    {
        public event Action GameDeath;
        public event Action GameLose;
        public event Action GameExit;

        public GameMain()
        {
            this.InitializeComponent();
            Translate();
            Player.Current.MoneyChanged += async () =>
            {
                CoreDispatcher dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    txtMoney.Text = Player.Current.Money.ToCurrencyString();
                });
            };

            dailyPlanner.NewDayCompleted += () =>
            {
                this.IsEnabled = true;
                progress.Visibility = Visibility.Collapsed;

                if (videoManager.Visibility == Visibility.Visible)
                    videoManager.Update();

                txtNewDay.Text = String.Format("{0} {1}!", EnglishStrings.StartDay.Translate(), Player.Current.Iterations + 1);

                if (Player.Current.Iterations > 1 && Player.Current.Iterations % 3 == 0)
                {
                    RandomEvents.RunEvent();
                    videoManager.Update();
                    dailyPlanner.Update();
                }
            };

            dailyPlanner.GameExit += () =>
            {
                if (GameLose != null)
                    GameLose();
            };
            dailyPlanner.Death += () =>
            {
                if (GameDeath != null)
                    GameDeath();
            };

            dailyPlanner.NewDay();

            btnDailyPlanner.HandPointerCursor();
            btnOnlineStore.HandPointerCursor();
            btnVideoPlanner.HandPointerCursor();
            imgSave.HandPointerCursor();
            gridHelp.HandPointerCursor();
            gridExit.HandPointerCursor();
            btnNewDay.HandPointerCursor();
        }

        private void Translate()
        {
            ToolTipService.SetToolTip(gridHelp, new ToolTip() { Content = EnglishStrings.Help.Translate() });
            ToolTipService.SetToolTip(gridExit, new ToolTip() { Content = EnglishStrings.Exit.Translate() });
            ToolTipService.SetToolTip(imgSave, new ToolTip() { Content = EnglishStrings.SaveGame.Translate() });
            txtDailyPlanner.Text = EnglishStrings.DailyPlanner.Translate();
            txtVideoPlanner.Text = EnglishStrings.VideoManager.Translate();
            txtOnlineStore.Text = EnglishStrings.OnlineStore.Translate();
        }

        private void btnDailyPlanner_Click(object sender, TappedRoutedEventArgs e)
        {
            btnDailyPlanner.ToggleClicked();

            btnDailyPlanner.IsChecked = true;
            btnVideoPlanner.IsChecked = false;
            btnOnlineStore.IsChecked = false;

            dailyPlanner.Visibility = Visibility.Visible;
            videoManager.Visibility = Visibility.Collapsed;
            onlineStore.Visibility = Visibility.Collapsed;
        }

        private void btnVideoPlanner_Click(object sender, TappedRoutedEventArgs e)
        {
            btnVideoPlanner.ToggleClicked();

            btnDailyPlanner.IsChecked = false;
            btnVideoPlanner.IsChecked = true;
            btnOnlineStore.IsChecked = false;

            dailyPlanner.Visibility = Visibility.Collapsed;
            videoManager.Visibility = Visibility.Visible;
            onlineStore.Visibility = Visibility.Collapsed;
            videoManager.Update();
        }

        private void btnOnlineStore_Click(object sender, TappedRoutedEventArgs e)
        {
            btnOnlineStore.ToggleClicked();

            btnDailyPlanner.IsChecked = false;
            btnVideoPlanner.IsChecked = false;
            btnOnlineStore.IsChecked = true;

            dailyPlanner.Visibility = Visibility.Collapsed;
            videoManager.Visibility = Visibility.Collapsed;
            onlineStore.Visibility = Visibility.Visible;
            onlineStore.Update();
        }

        private void btnNewDay_Click(object sender, TappedRoutedEventArgs e)
        {
            this.IsEnabled = false;
            progress.Visibility = Visibility.Visible;

            dailyPlanner.NewDay();
        }

        private async void Tutorial_MouseLeftButtonDown(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.youtube.com/watch?v=oKfNSm1SLSQ"));
        }

        private async void Save_MouseLeftButtonDown(object sender, TappedRoutedEventArgs e)
        {
            if (await SaveLoadHelper.SaveExistsAsync())
            {
                CustomMessageBox.ShowDialog(EnglishStrings.OverwriteSave.Translate(), EnglishStrings.SaveExists.Translate(), MessagePicture.Question, (result) =>
                {
                    if (result == true)
                        DoSave();
                });
            }
            else
            {
                DoSave();
            }
        }

        private void DoSave()
        {
            foreach (var a in dailyPlanner.Appointments)
            {
                a.HoursPutIn--;
            }
            SaveLoadHelper.Save(SaveLoadHelper.SaveFile);
            foreach (var a in dailyPlanner.Appointments)
            {
                a.HoursPutIn++;
            }
            CustomMessageBox.ShowDialog(EnglishStrings.SaveGame.Translate(), EnglishStrings.SaveGameSuccess.Translate(), MessagePicture.Happy);
        }

        private void Exit_MouseLeftButtonDown(object sender, TappedRoutedEventArgs e)
        {
            CustomMessageBox.ShowDialog(EnglishStrings.Confirm.Translate(), EnglishStrings.LeaveGame.Translate(), MessagePicture.Question, (result) =>
            {
                if (result == true)
                {
                    if (GameExit != null)
                        GameExit();
                }
            });
        }
    }
}