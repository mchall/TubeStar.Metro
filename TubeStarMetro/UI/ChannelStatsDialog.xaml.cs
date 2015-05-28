using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TubeStarMetro
{
    public sealed partial class ChannelStatsDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public ChannelStatsDialog(Channel channel)
        {
            InitializeComponent();
            Translate();
            Populate(channel);

            btnOk.HandPointerCursor();
            this.HandleDialogKeyboard(OKButton_Click, OKButton_Click);
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
            //Caption = EnglishStrings.Stats.Translate();
            btnOk.Content = EnglishStrings.Ok.Translate();
        }

        private void Populate(Channel channel)
        {
            PopulateSubscribers(channel);
            PopulateIncome(channel);
        }

        private void PopulateSubscribers(Channel channel)
        {
            TextBlock nameBlock = new TextBlock();
            nameBlock.Text = EnglishStrings.SubscriberHistory.Translate();
            nameBlock.FontSize = 16;
            nameBlock.Margin = new Thickness(5, 0, 5, 0);
            plotParent.Children.Add(nameBlock);

            OxyPlot.Metro.Plot plot = new OxyPlot.Metro.Plot();
            plot.Height = 200;

            var plotModel1 = new PlotModel();
            var viewSeries = new LineSeries(OxyColors.Blue);
            int count = 1;
            foreach (var subscriber in channel.SubscribersOverTime)
            {
                viewSeries.Points.Add(new DataPoint(count++, subscriber * InternetGraph.Multiplier));
            }
            plotModel1.Series.Add(viewSeries);
            plot.Model = plotModel1;
            plotParent.Children.Add(plot);
        }

        private void PopulateIncome(Channel channel)
        {
            TextBlock nameBlock = new TextBlock();
            nameBlock.Text = EnglishStrings.DailyIncome.Translate();
            nameBlock.FontSize = 16;
            nameBlock.Margin = new Thickness(5, 0, 5, 0);
            plotParent.Children.Add(nameBlock);

            OxyPlot.Metro.Plot plot = new OxyPlot.Metro.Plot();
            plot.Height = 200;

            var plotModel1 = new PlotModel();
            var incomeSeries = new LineSeries(OxyColors.Blue);
            int count = 1;
            double baseIncome = 0;
            foreach (var income in channel.IncomeOverTime)
            {
                baseIncome += income;
                incomeSeries.Points.Add(new DataPoint(count++, baseIncome));
            }
            plotModel1.Series.Add(incomeSeries);

            var expenseSeries = new LineSeries(OxyColors.Red);
            count = 1;
            double baseExpenses = 0;
            foreach (var expense in channel.ExpensesOverTime)
            {
                baseExpenses += expense;
                expenseSeries.Points.Add(new DataPoint(count++, baseExpenses));
            }
            plotModel1.Series.Add(expenseSeries);
            plot.Model = plotModel1;
            plotParent.Children.Add(plot);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(false);
        }
    }
}