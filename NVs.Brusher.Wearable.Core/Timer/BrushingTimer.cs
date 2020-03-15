using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NVs.Brusher.Wearable.Core.Annotations;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer.Actions;

namespace NVs.Brusher.Wearable.Core.Timer
{
    public sealed partial class BrushingTimer : INotifyPropertyChanged
    {
        private readonly INotificator notificator;
        private readonly object thisLock = new object();
        private volatile bool stateChangeIsInProgress;

        private TimerState state = TimerState.Stopped;
        private TimeSpan? remainingDuration;
        private System.Threading.Timer timer;

        public BrushingTimer(INotificator notificator)
        {
            this.notificator = notificator;
        }

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

        public TimeSpan? RemainingDuration
        {
            get => remainingDuration;
            private set
            {
                if (Nullable.Equals(value, remainingDuration)) return;
                remainingDuration = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event ExceptionRaisedEventHandler AsyncExceptionRaised;

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
            if (State == TimerState.Running)
            {
                return;
            }

            lock (thisLock)
            {
                if (stateChangeIsInProgress)
                {
                    return;
                }

                stateChangeIsInProgress = true;
            }

            try
            {
                State = TimerState.Running;
                if (timer == null)
                {
                    InitializeTimer();
                }

                timer.Change(Settings.HeartBitInterval, Settings.HeartBitInterval);
            }
            finally
            {
                lock (thisLock) stateChangeIsInProgress = false;
            }
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
            if (timer != null)
            {
                lock (thisLock)
                {
                    if (stateChangeIsInProgress)
                    {
                        return;
                    }

                    stateChangeIsInProgress = true;
                }
            }

            try
            {
                if (timer != null)
                {
                    timer.Dispose();
                    RemainingDuration = null;
                }

                State = TimerState.Stopped;
            }
            finally
            {
                lock (thisLock)
                {
                    stateChangeIsInProgress = false;
                }
            }
        }

        private void InitializeTimer()
        {
            var context = new TickContext(this);
            var schedule = context.Actions;

            schedule.Push(new NotifyTimerFinishedTimerAction());

            AddDelays(Settings.PolishingSettings, schedule);
            AddDelays(Settings.CleaningSettings, schedule);
            AddDelays(Settings.SweepingSettings, schedule);

            schedule.Push(new NotifyTimerStartedTimerAction());

            var duration = TimeSpan.Zero;
            foreach (var action in schedule)
            {
                switch (action)
                {
                    case CountdownTimerAction a:
                        duration = duration.Add(a.Delay);
                        break;
                }
            }

            RemainingDuration = duration;
            timer = new System.Threading.Timer(OnTick, context, TimeSpan.FromMilliseconds(-1),
                Settings.HeartBitInterval);
        }

        private void AddDelays(IntervalSettings intervalSettings, Stack<TimerAction> schedule)
        {
            if (!intervalSettings.Enabled)
            {
                return;
            }

            if (schedule.Count > 1)
            {
                schedule.Push(new NotifyStageChangedTimerAction());
            }

            for (var i = intervalSettings.Repeats - 1; i >= 0 ; i--)
            {
                schedule.Push(new CountdownTimerAction(intervalSettings.Delay));
            }

        }


        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public delegate void ExceptionRaisedEventHandler(object sender, ExceptionRaisedEventArgs args);

    public sealed class ExceptionRaisedEventArgs : EventArgs
    {
        public ExceptionRaisedEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}