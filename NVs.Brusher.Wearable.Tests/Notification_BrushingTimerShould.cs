﻿using System;
using System.Threading.Tasks;
using Moq;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public class Notification_BrushingTimerShould
    {
        [Fact]
        public async void NotifyWhenProgramEnds()
        {
            var notificator = new Mock<INotificator>();

            var timer = new BrushingTimer(notificator.Object).WithSettings(new BrushingSettings()
            {
                CleaningSettings = { Enabled = true, Delay = TimeSpan.FromMilliseconds(100), Repeats = 1},
                HeartBitInterval = TimeSpan.FromMilliseconds(100)
            });

            timer.Start();
            await Task.Delay(timer.RemainingDuration.Value + 2*timer.Settings.HeartBitInterval);

            Assert.Equal(TimerState.Stopped, timer.State);
            notificator.Verify(x => x.NotifyTimerFinished(), Times.Once);

        }
    }
}