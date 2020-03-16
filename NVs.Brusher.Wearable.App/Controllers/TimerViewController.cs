using System;
using NVs.Brusher.Wearable.App.Views;

namespace NVs.Brusher.Wearable.App.Controllers
{
    sealed class TimerViewController
    {
        public TimerViewController(ITimerView timerView)
        {
            timerView.ToggleStateButtonPressed += OnToggleStateButtonPressed;
        }

        private void OnToggleStateButtonPressed(object sender, EventArgs e)
        {
        }

        public void HandleKeyEvent(object sender, KeyEventArgs keyEventArgs)
        {
        }
    }
}