using System;
using System.Collections.Generic;
using NVs.Brusher.Wearable.Core.Timer.Actions;

namespace NVs.Brusher.Wearable.Core.Timer
{
    public sealed partial class BrushingTimer
    {
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

            public TimeSpan HeartbBitInterval => timer.Settings.HeartBitInterval;

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

            public void NotifyTimerStarted()
            {
                timer.notificator.NotifyTimerStarted();
            }

            public void Stop()
            {
                timer.Stop();
            }
        }

        private void OnTick(object o)
        {
            if (!(o is TickContext context))
            {
                return;
            }


            try
            {
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

                        case NotifyTimerStartedTimerAction _:
                            context.NotifyTimerStarted();
                            break;
                    }
                }

                context.CurrentDelay = context.CurrentDelay.Value.Subtract(context.HeartbBitInterval);
                context.DecreaseRemaining();

                if (context.CurrentDelay <= TimeSpan.Zero)
                {
                    context.CurrentDelay = null;
                }
            }
            catch (Exception e)
            {
                RaiseAsyncExceptionRaised(new ExceptionRaisedEventArgs(e));
            }
        }

        private void RaiseAsyncExceptionRaised(ExceptionRaisedEventArgs args)
        {
            AsyncExceptionRaised?.Invoke(this, args);
        }
    }
}