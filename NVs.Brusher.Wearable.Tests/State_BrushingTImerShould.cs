using Moq;
using NVs.Brusher.Wearable.Core.Timer;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class State_BrushingTimerShould
    {
        private static readonly INotificator Notificator = new Mock<INotificator>().Object;

        [Fact]
        public void BeStoppedByDefault()
        {
            var timer = new BrushingTimer(Notificator);
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void BeRunningIfWasStarted()
        {
            var timer = new BrushingTimer(Notificator);
            timer.Start();

            Assert.Equal(TimerState.Running, timer.State);
        }

        [Fact]
        public void BePausedIfWasPaused()
        {
            var timer = new BrushingTimer(Notificator);
            timer.Start();
            timer.Pause();

            Assert.Equal(TimerState.Paused, timer.State);
        }

        [Fact]
        public void BeStoppedIfWasStoppedBeingRunning()
        {
            var timer = new BrushingTimer(Notificator);
            timer.Start();

            timer.Stop();
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void BeStoppedIfWasStoppedBeingPaused()
        {
            var timer = new BrushingTimer(Notificator);
            timer.Start();
            timer.Pause();

            timer.Stop();
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void StayStoppedIfWasPausedBeingStopped()
        {
            var timer = new BrushingTimer(Notificator);
            timer.Pause();

            Assert.Equal(TimerState.Stopped, timer.State);
        }
    }
}