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
            State = TimerState.Running;
        }

        public void Pause()
        {
            if (State != TimerState.Running)
            {
                return;
            }

            State = TimerState.Paused;
        }

        public void Stop()
        {
            State = TimerState.Stopped;
        }

        

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}