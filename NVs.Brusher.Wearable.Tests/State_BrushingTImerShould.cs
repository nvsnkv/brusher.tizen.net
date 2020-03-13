using NVs.Brusher.Wearable.Core.Timer;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class State_BrushingTimerShould
    {
        [Fact]
        public void BeStoppedByDefault()
        {
            var timer = new BrushingTimer();
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void BeRunningIfWasStarted()
        {
            var timer = new BrushingTimer();
            timer.Start();

            Assert.Equal(TimerState.Running, timer.State);
        }

        [Fact]
        public void BePausedIfWasPaused()
        {
            var timer = new BrushingTimer();
            timer.Start();
            timer.Pause();

            Assert.Equal(TimerState.Paused, timer.State);
        }

        [Fact]
        public void BeStoppedIfWasStoppedBeingRunning()
        {
            var timer = new BrushingTimer();
            timer.Start();

            timer.Stop();
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void BeStoppedIfWasStoppedBeingPaused()
        {
            var timer = new BrushingTimer();
            timer.Start();
            timer.Pause();

            timer.Stop();
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void StayStoppedIfWasPausedBeingStopped()
        {
            var timer = new BrushingTimer();
            timer.Pause();

            Assert.Equal(TimerState.Stopped, timer.State);
        }
    }
}