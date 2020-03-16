using System;
using NVs.Brusher.Wearable.App.Views;
using NVs.Brusher.Wearable.Core.Timer;
using Tizen.Maps;

namespace NVs.Brusher.Wearable.App.Controllers
{
    sealed class TimerViewController
    {
        private readonly BrushingTimer brushingTimer;

        public TimerViewController(ITimerView timerView, BrushingTimer brushingTimer)
        {
            this.brushingTimer = brushingTimer;
            timerView.ToggleStateButtonPressed += OnToggleStateButtonPressed;
        }

        private void OnToggleStateButtonPressed(object sender, EventArgs e)
        {
            switch (brushingTimer.State)
            {
                case TimerState.Stopped:
                    brushingTimer.Start();
                    break;
                case TimerState.Paused:
                    brushingTimer.Start();
                    break;
                case TimerState.Running:
                    brushingTimer.Pause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void HandleKeyEvent(object sender, KeyEventArgs keyEventArgs)
        {
            switch (brushingTimer.State)
            {
                case TimerState.Stopped:
                    keyEventArgs.Handled = false;
                    break;
                case TimerState.Paused:
                    brushingTimer.Stop();
                    break;
                case TimerState.Running:
                    brushingTimer.Pause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}