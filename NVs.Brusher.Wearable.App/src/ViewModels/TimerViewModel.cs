using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NVs.Brusher.Wearable.Core;
using Xamarin.Forms;

namespace NVs.Brusher.Wearable.App.ViewModels
{
    public sealed class TimerViewModel : INotifyPropertyChanged
    {
        private readonly MultiTimer timer;

        public TimerViewModel(MultiTimer timer)
        {
            this.timer = timer;
            timer.PropertyChanged += OnTimerPropertyChanged;

            ToggleState = new ToggleTimerStateCommand(timer);
        }

        public long RemainingTicks => timer.RemainingTicks ?? 0;

        public double Progress => (double)RemainingTicks / timer.TotalTicks;

        public TimerState State => timer.State;

        public ToggleTimerStateCommand ToggleState { get; }

        private void OnTimerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MultiTimer.State):
                    RaisePropertyChanged(nameof(State));
                    break;

                case nameof(MultiTimer.RemainingTicks):
                    RaisePropertyChanged(nameof(RemainingTicks));
                    RaisePropertyChanged(nameof(Progress));
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}