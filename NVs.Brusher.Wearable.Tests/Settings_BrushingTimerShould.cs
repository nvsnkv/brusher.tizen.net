﻿using System;
using System.Threading.Tasks;
using Moq;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class Settings_BrushingTimerShould
    {
        private static readonly INotificator Notificator = new Mock<INotificator>().Object;

        [Fact]
        public void HaveDefaultSettingsAfterCreation()
        {
            var timer = new BrushingTimer(Notificator);
            Assert.Equal(timer.Settings, BrushingSettings.Default);
        }


        [Fact]
        public void AllowToChangeSettingsWhileStopped()
        {
            var timer = new BrushingTimer(Notificator);
            timer.SetSettings(new BrushingSettings());

            Assert.Equal(new BrushingSettings(), timer.Settings);
        }

        [Fact]
        public void NotAllowToChangeSettingsIfStarted()
        {
            var timer = new BrushingTimer(Notificator);
            timer.Start();

            Assert.Throws<InvalidOperationException>(() => timer.SetSettings(new BrushingSettings()));
        }

        [Fact]
        public void NotAllowToChangeSettingsIfPaused()
        {
            var timer = new BrushingTimer(Notificator);
            timer.Start();
            timer.Pause();

            Assert.Throws<InvalidOperationException>(() => timer.SetSettings(new BrushingSettings()));
        }
    }
}