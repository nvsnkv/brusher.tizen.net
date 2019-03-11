using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NVs.Brusher.Wearable.Models.Annotations;

namespace NVs.Brusher.Wearable.Models
{
    public sealed class MultiTimer : INotifyPropertyChanged
    {
        private readonly int[] intervals;
        private readonly long total;
        private readonly HeartBit heartBit;
        private long? totalRemainingTicks;
        private volatile TimerState state;
        private int intervalRemaining = -1;
        private int intervalIndex = -1;

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

        public long? RemainingTicks
        {
            get => totalRemainingTicks;
            private set
            {
                if (value == totalRemainingTicks) return;
                totalRemainingTicks = value;
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
            
            var previousState = State;
            State = TimerState.Running;

            if (previousState == TimerState.Stopped)
            {
                RemainingTicks = total;
                intervalIndex = 0;
                intervalRemaining = intervals[intervalIndex];
            }

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
            RemainingTicks = null;
        }

        private void TickHandler(object sender, EventArgs e)
        {
            if (State != TimerState.Running) { return; }
            
            RemainingTicks--;

            if (intervalRemaining == 0)
            {
                TimeoutKind timeoutKind;

                switch (intervalIndex)
                {
                    case int i when i == 0:
                        timeoutKind = TimeoutKind.First;
                        break;

                    case int i when i == intervals.Length - 1:
                        timeoutKind = TimeoutKind.Last;
                        break;

                    default:
                        timeoutKind = TimeoutKind.Subsequent;
                        break;
                }

                RaiseTimeout(timeoutKind);

                if (intervalIndex < intervals.Length - 1)
                {
                    intervalIndex++;
                    intervalRemaining = intervals[intervalIndex];
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                intervalRemaining--;
            }
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