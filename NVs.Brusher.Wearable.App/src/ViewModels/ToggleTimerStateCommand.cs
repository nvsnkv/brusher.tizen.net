using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NVs.Brusher.Wearable.Core;

namespace NVs.Brusher.Wearable.App.ViewModels
{
    public sealed class ToggleTimerStateCommand : INotifyPropertyChanged, ICommand
    {
        private readonly MultiTimer timer;
        private string title;

        public ToggleTimerStateCommand(MultiTimer timer)
        {
            this.timer = timer;
            this.timer.PropertyChanged += TimerOnPropertyChanged;
            UpdateTitle();
        }

        public string Title
        {
            get => title;
            private set
            {
                if (value == title) return;
                title = value;
                RaisePropertyChanged();
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch (timer.State)
            {
                case TimerState.Stopped:
                    timer.Start();
                    break;
                case TimerState.Running:
                    timer.Pause();
                    break;
                case TimerState.Paused:
                    timer.Start();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler CanExecuteChanged;

        private void TimerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MultiTimer.State))
            {
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            switch (timer.State)
            {
                case TimerState.Stopped:
                    Title = "Start";
                    break;
                case TimerState.Running:
                    Title = "Pause";
                    break;
                case TimerState.Paused:
                    Title = "Resume";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Annotations.NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}