using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace TubeStarMetro
{
    public sealed partial class AppointmentBlock : UserControl
    {
        public string Hour { get; set; }
        public string Minute { get; set; }

        private TaskBase _task;
        public TaskBase Task
        {
            get { return _task; }
            set
            {
                _task = value;
                appointmentGrid.Opacity = (_task == null) ? 0 : 1;
                if (_task != null)
                {
                    appointmentGrid.Background = new SolidColorBrush(_task.Color);
                    txtTime.Text = String.Format("{0} {1}", Hour, Minute);
                    appointmentTextBlock.Text = _task.Name;
                }
            }
        }

        public AppointmentBlock()
        {
            this.InitializeComponent();
            this.HandPointerCursor();
        }
    }
}