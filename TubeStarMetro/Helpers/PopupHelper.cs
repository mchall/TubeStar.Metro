using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace TubeStarMetro
{
    public interface IPopupControl
    {
        void SetParent(PopupHelper parent);
        void OnFocus();
    }

    public class PopupHelper
    {
        private Popup _popup;
        private UserControl _child;
        private IPopupControl _childInterface;
        private TaskCompletionSource<bool> _showCompleteTaskSource;

        public bool? DialogResult { get; private set; }

        public PopupHelper(UserControl control)
        {
            _child = control;
            _childInterface = _child as IPopupControl;
        }

        protected void CallControl(Action<IPopupControl> pred)
        {
            if (_childInterface != null)
            {
                pred(_childInterface);
            }
        }

        public void Close(bool? dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        protected void Close()
        {
            _popup.IsOpen = false;
            _showCompleteTaskSource.TrySetResult(true);
        }

        public async Task ShowAsync()
        {
            if (_popup != null)
            {
                throw new InvalidOperationException();
            }
            _popup = new Popup();
            _popup.IsLightDismissEnabled = true;
            _popup.Height = Window.Current.Bounds.Height;
            _popup.Width = Window.Current.Bounds.Width;
            _popup.GotFocus += _popup_GotFocus;

            _child.Height = _popup.Height;
            _child.Width = _popup.Width;
            _popup.Child = _child;

            CallControl(x => x.SetParent(this));

            _popup.IsOpen = true;

            await WaitForClosureAsync();
        }

        private void _popup_GotFocus(object sender, RoutedEventArgs e)
        {
            CallControl(x => x.OnFocus());
        }

        protected virtual Task WaitForClosureAsync()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            _showCompleteTaskSource = tcs;
            return tcs.Task;
        }

        private void Cleanup()
        {
            _popup = null;
        }
    }
}