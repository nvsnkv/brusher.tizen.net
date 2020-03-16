using Microsoft.Extensions.Logging;
using Moq;
using NVs.Brusher.Wearable.Core.Timer;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class State_BrushingTimerShould
    {
        private static readonly INotificator Notificator = new Mock<INotificator>().Object;
        private static readonly ILogger<BrushingTimer> Logger = new Mock<ILogger<BrushingTimer>>().Object;

        [Fact]
        public void BeStoppedByDefault()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void BeRunningIfWasStarted()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.Start();

            Assert.Equal(TimerState.Running, timer.State);
        }

        [Fact]
        public void BePausedIfWasPaused()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.Start();
            timer.Pause();

            Assert.Equal(TimerState.Paused, timer.State);
        }

        [Fact]
        public void BeStoppedIfWasStoppedBeingRunning()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.Start();

            timer.Stop();
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void BeStoppedIfWasStoppedBeingPaused()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.Start();
            timer.Pause();

            timer.Stop();
            Assert.Equal(TimerState.Stopped, timer.State);
        }

        [Fact]
        public void StayStoppedIfWasPausedBeingStopped()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.Pause();

            Assert.Equal(TimerState.Stopped, timer.State);
        }
    }
}