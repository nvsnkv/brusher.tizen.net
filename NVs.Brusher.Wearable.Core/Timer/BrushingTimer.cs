using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NVs.Brusher.Wearable.Core.Annotations;
using NVs.Brusher.Wearable.Core.Settings;

namespace NVs.Brusher.Wearable.Core.Timer
{
    public sealed class BrushingTimer : INotifyPropertyChanged
    {
        private TimerState state = TimerState.Stopped;
        
        public BrushingSettings Settings { get; private set; } = BrushingSettings.Default;

        public TimerState State
        {
            get => state;
            private set
            {
                if (value == state) return;
                state = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetSettings(BrushingSettings settings)
        {
            if (State != TimerState.Stopped)
            {
                throw new InvalidOperationException("Change of settings is not allowed while timer is running");
            }

            Settings = settings;
        }

        public void Start()
        {
            UpdateStateTo(TimerState.Running);
        }

        public void Pause()
        {
            UpdateStateTo(TimerState.Paused);
        }

        private void UpdateStateTo(TimerState newState)
        {
            if (state == newState)
            {
                return;
            }

            switch (state)
            {
                case TimerState.Stopped:
                    if (newState == TimerState.Paused)
                    {
                        return;
                    }
                    break;
                case TimerState.Paused:
                    break;
                case TimerState.Running:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            State = newState;
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}