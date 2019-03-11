using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NVs.Brusher.Wearable.Models.Annotations;

namespace NVs.Brusher.Wearable.Models
{
    public sealed class MultiTimer: INotifyPropertyChanged
    {
        private readonly int[] intervals;
        private readonly long total;
        private readonly HeartBit heartBit;
        private long? totalRemainingSeconds;
        private TimerState state;
        
        public MultiTimer([NotNull] HeartBit heartBit, [NotNull] int[] intervals)
        {
            if (heartBit == null) throw new ArgumentNullException(nameof(heartBit));
            if (intervals == null) throw new ArgumentNullException(nameof(intervals));
            if (intervals.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(intervals));

            this.intervals = intervals;
            this.total = intervals.Sum() + intervals.Length;
            this.heartBit = heartBit;
            this.heartBit.Tick += TickHandler;
        }

        public long? RemainingSeconds
        {
            get => totalRemainingSeconds;
            private set
            {
                if (value == totalRemainingSeconds) return;
                totalRemainingSeconds = value;
                RaisePropertyChanged();
            }
        }

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

        public event EventHandler<TimeoutKind> TimeoutOccured;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Start()
        {
            if (State == TimerState.Running) { return; }

            if (State == TimerState.Stopped)
            {
                RemainingSeconds = total;
            }

            State = TimerState.Running;
            heartBit.Start();
        }

        public void Pause()
        {
            if (State != TimerState.Running) { return; }
            State = TimerState.Paused;

            heartBit.Stop();
        }

        public void Stop()
        {
            if (State == TimerState.Stopped) { return; }
            State = TimerState.Stopped;

            heartBit.Stop();
            RemainingSeconds = null;
        }

        private void TickHandler(object sender, EventArgs e)
        {
            if (State != TimerState.Running) { return; }

            throw new NotImplementedException();
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseTimeout(TimeoutKind e)
        {
            TimeoutOccured?.Invoke(this, e);
        }
    }
}