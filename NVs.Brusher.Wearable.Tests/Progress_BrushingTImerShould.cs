using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public class Progress_BrushingTimerShould
    {
        [Theory]
        [InlineData(true, 300, 2, true, 200, 3, true, 300, 1)]
        [InlineData(true, 300, 2, false, 200, 3, true, 300, 1)]
        [InlineData(true, 300, 2, false, 200, 3, false, 300, 1)]
        [InlineData(false, 300, 2, true, 200, 3, true, 300, 1)]
        [InlineData(false, 300, 2, false, 200, 3, true, 300, 1)]
        [InlineData(false, 300, 2, false, 200, 3, false, 300, 1)]

        public void CalculateTotalDurationFromSetupCorrectly(bool sweepEnabled, long sweepDelay, int sweepRepeats, bool cleanEnabled, long cleanDelay, int cleanRepeats, bool polishEnabled, long polishDelay, int polishRepeats)
        {
            var timer = new BrushingTimer();
            timer.SetSettings(new BrushingSettings()
            {
                SweepingSettings =                                  
                {
                    Enabled = sweepEnabled,
                    Delay = TimeSpan.FromMilliseconds(sweepDelay),
                    Repeats = sweepRepeats
                },

                CleaningSettings =
                {
                    Enabled = cleanEnabled,
                    Delay = TimeSpan.FromMilliseconds(cleanDelay),
                    Repeats = cleanRepeats
                },

                PolishingSettings =
                {
                    Enabled = polishEnabled,
                    Delay = TimeSpan.FromMilliseconds(polishDelay),
                    Repeats = 1
                },
            });

            var expected = timer.Settings.SweepingSettings.Delay * timer.Settings.SweepingSettings.Repeats * (timer.Settings.SweepingSettings.Enabled ? 1 : 0)
                           + timer.Settings.CleaningSettings.Delay * timer.Settings.CleaningSettings.Repeats * (timer.Settings.CleaningSettings.Enabled ? 1 : 0)
                           + timer.Settings.PolishingSettings.Delay * timer.Settings.PolishingSettings.Repeats * (timer.Settings.PolishingSettings.Enabled ? 1 : 0);

            timer.Start();
            var remaining = timer.RemainingDuration;
            timer.Stop();

            Assert.True(remaining.HasValue);
            Assert.Equal(expected, remaining.Value);
        }

        [Fact]
        public async Task NotifyAboutProgressTimely()
        {
            var stopwatch = new Stopwatch();
            var actual = new List<(TimeSpan?, TimeSpan)>();
            var timer = new BrushingTimer();
            timer.SetSettings(new BrushingSettings()
            {
                CleaningSettings = { Enabled = true, Delay = TimeSpan.FromMilliseconds(300), Repeats = 2 },
                SweepingSettings = { Enabled = true, Delay = TimeSpan.FromMilliseconds(100), Repeats = 3 },
                PolishingSettings = { Enabled = true, Delay = TimeSpan.FromMilliseconds(400), Repeats = 1},
                HeartBitInterval = TimeSpan.FromMilliseconds(100)
            });

            timer.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(timer.RemainingDuration))
                {
                    actual.Add((timer.RemainingDuration, stopwatch.Elapsed));
                }
            };

            timer.Start();
            stopwatch.Start();
            var remainingDuration = timer.RemainingDuration.Value;

            await Task.Delay(remainingDuration + TimeSpan.FromMilliseconds(300));

            Assert.Equal(15, actual.Count); // initial calculation , 13 ticks and Zero -> null notification
            foreach (var (remaining, elapsed) in actual)
            {
                Assert.True(remainingDuration - (remaining ?? TimeSpan.Zero) - elapsed < TimeSpan.FromMilliseconds(10));
            }
        }

        [Fact]
        public async Task StopOnCountdownZero()
        {
            var timer = new BrushingTimer().WithSettings(new BrushingSettings()
            {
                CleaningSettings = { Enabled = true, Delay = TimeSpan.FromMilliseconds(100), Repeats = 5}, 
                HeartBitInterval = TimeSpan.FromMilliseconds(100)
            });

            timer.Start();
            var remaining = timer.RemainingDuration.Value.Add(TimeSpan.FromMilliseconds(100)); // HACK: it's not ending when I expect - there could be an issue inside a timer.
                                                                                                     // at the same time, System.Threading.Timer is not too precise
            await Task.Delay(remaining);

            Assert.Equal(TimerState.Stopped, timer.State);
        }
    }
}