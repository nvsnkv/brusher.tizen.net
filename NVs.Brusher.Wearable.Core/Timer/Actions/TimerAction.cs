using System;

namespace NVs.Brusher.Wearable.Core.Timer.Actions
{
    abstract class TimerAction
    {

    }

    sealed class NotifyStageChangedTimerAction : TimerAction
    {
    }

    sealed class NotifyTimerFinishedTimerAction : TimerAction
    {
    }

    sealed class CountdownTimerAction : TimerAction
    {
        public CountdownTimerAction(TimeSpan delay)
        {
            Delay = delay;
        }

        public TimeSpan Delay { get; }
    }
}