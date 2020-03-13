using System;

namespace NVs.Brusher.Wearable.Core.Timer.Actions
{
    abstract class TimerAction
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