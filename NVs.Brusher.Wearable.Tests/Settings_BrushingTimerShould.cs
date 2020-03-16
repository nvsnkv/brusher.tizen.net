using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class Settings_BrushingTimerShould
    {
        private static readonly INotificator Notificator = new Mock<INotificator>().Object;
        private static readonly ILogger<BrushingTimer> Logger = new Mock<ILogger<BrushingTimer>>().Object;

        [Fact]
        public void HaveDefaultSettingsAfterCreation()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            Assert.Equal(timer.Settings, BrushingSettings.Default);
        }


        [Fact]
        public void AllowToChangeSettingsWhileStopped()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.SetSettings(new BrushingSettings());

            Assert.Equal(new BrushingSettings(), timer.Settings);
        }

        [Fact]
        public void NotAllowToChangeSettingsIfStarted()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.Start();

            Assert.Throws<InvalidOperationException>(() => timer.SetSettings(new BrushingSettings()));
        }

        [Fact]
        public void NotAllowToChangeSettingsIfPaused()
        {
            var timer = new BrushingTimer(Notificator, Logger);
            timer.Start();
            timer.Pause();

            Assert.Throws<InvalidOperationException>(() => timer.SetSettings(new BrushingSettings()));
        }
    }
}