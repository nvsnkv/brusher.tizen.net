using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NVs.Brusher.Wearable.Core.Annotations;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer.Actions;

namespace NVs.Brusher.Wearable.Core.Timer
{
    public sealed class BrushingTimer : INotifyPropertyChanged
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
            AddDelays(Settings.PolishingSettings, schedule);
            AddDelays(Settings.CleaningSettings, schedule);
            AddDelays(Settings.SweepingSettings, schedule);

            if (schedule.Count > 0)
            {
                schedule.Pop();
                schedule.Push(new NotifyTimerFinishedTimerAction());
            }

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
            timer = new System.Threading.Timer(OnTick, context, TimeSpan.FromMilliseconds(-1), Settings.HeartBitInterval);
        }

        private void OnTick(object o)
        {
            if (!(o is TickContext context))
            {
                return;
            }

            while (!context.CurrentDelay.HasValue)
            {
                if (!context.TimerRunning)
                {
                    return;
                }

                if (context.Actions.Count == 0)
                {
                    context.Stop();
                    return;
                }

                var action = context.Actions.Pop();
                switch (action)
                {
                    case CountdownTimerAction countdown:
                        context.CurrentDelay = countdown.Delay;
                        break;

                    case NotifyStageChangedTimerAction _:
                        context.NotifyStageChanged();
                        break;

                    case NotifyTimerFinishedTimerAction _:
                        context.NotifyTimerFinished();
                        break;
                }
            }

            context.CurrentDelay = context.CurrentDelay.Value.Subtract(context.HeartbitInterval);
            context.DecreaseRemaining();

            if (context.CurrentDelay <= TimeSpan.Zero)
            {
                context.CurrentDelay = null;
            }
        }

        private void AddDelays(IntervalSettings intervalSettings, Stack<TimerAction> schedule)
        {
            if (!intervalSettings.Enabled)
            {
                return;
            }

            for (var i = 0; i < intervalSettings.Repeats; i++)
            {
                schedule.Push(new CountdownTimerAction(intervalSettings.Delay));
            }
            schedule.Push(new NotifyStageChangedTimerAction());
        }


        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class TickContext
        {
            private readonly BrushingTimer timer;

            public TickContext(BrushingTimer timer)
            {
                this.timer = timer;
            }

            public TimeSpan? CurrentDelay { get; set; }

            public Stack<TimerAction> Actions { get; } = new Stack<TimerAction>();

            public bool TimerRunning => timer.State == TimerState.Running;

            public TimeSpan HeartbitInterval => timer.Settings.HeartBitInterval;

            public void DecreaseRemaining()
            {
                if (!timer.RemainingDuration.HasValue)
                {
                    return;
                }

                timer.RemainingDuration = timer.RemainingDuration.Value.Subtract(timer.Settings.HeartBitInterval);
            }

            public void NotifyStageChanged()
            {
                timer.notificator.NotifyStageChanged();
            }

            public void NotifyTimerFinished()
            {
                timer.notificator.NotifyTimerFinished();
            }

            public void Stop()
            {
                timer.Stop();
            }
        }
    }
}