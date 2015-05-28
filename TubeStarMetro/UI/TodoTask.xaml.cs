using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TubeStarMetro
{
    public sealed partial class TodoTask : UserControl
    {
        public event EventHandler CancelTaskClick;

        private TaskBase _task;
        public TaskBase Task
        {
            get { return _task; }
            private set
            {
                _task = value;
                todoGrid.Opacity = (_task == null) ? 0 : 1;
                if (_task != null)
                {
                    todoGrid.Background = new SolidColorBrush(_task.Color);
                    UpdateText();
                }
            }
        }

        public TodoTask(TaskBase task)
        {
            InitializeComponent();
            Task = task;
            Translate();

            this.HandPointerCursor();
            btnCancel.HandPointerCursor();
        }

        private void Translate()
        {
            btnCancel.Content = EnglishStrings.Cancel.Translate();
        }

        public void UpdateText()
        {
            if (_task != null)
            {
                appointmentTextBlock.Text = _task.Name;
                txtHoursLeft.Text = String.Format("{0}: {1}", EnglishStrings.HoursLeft.Translate(), _task.HoursLeft);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CancelTaskClick != null)
                CancelTaskClick(this, EventArgs.Empty);
        }
    }
}