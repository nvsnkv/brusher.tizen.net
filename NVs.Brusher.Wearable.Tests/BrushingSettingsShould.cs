using System;
using System.Runtime.InteropServices.WindowsRuntime;
using NVs.Brusher.Wearable.Core.Settings;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class BrushingSettingsShould
    {
        [Fact]
        public void BeEqualIfSetupIsEqual()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();

            var result = left.Equals(right);
            Assert.True(result);
        }

        [Fact]
        public void BeDifferentIfSweepingSettingsAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.SweepingSettings.Enabled = !left.SweepingSettings.Enabled;

            Assert.False(Equals(right.SweepingSettings, left.SweepingSettings));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfCleaningSettingsAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.CleaningSettings.Duration = left.CleaningSettings.Duration * 2;

            Assert.False(Equals(right.CleaningSettings, left.CleaningSettings));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfPolishingSettingsAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;

            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfHeartBitsAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.HeartBitInterval = 3 * left.HeartBitInterval;

            Assert.False(Equals(right.HeartBitInterval, left.HeartBitInterval));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfPolishingAndSweepingSettingsAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;
            right.SweepingSettings.Enabled = !left.SweepingSettings.Enabled;

            Assert.False(Equals(right.SweepingSettings, left.SweepingSettings));
            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfPolishingAndCleaningSettingsAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;
            right.CleaningSettings.Enabled = !left.CleaningSettings.Enabled;

            Assert.False(Equals(right.CleaningSettings, left.CleaningSettings));
            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfPolishingAndHeartBitAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;
            right.HeartBitInterval = 3 * left.HeartBitInterval;

            Assert.False(Equals(right.HeartBitInterval, left.HeartBitInterval));
            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfPolishingAndSweepingSettingsAndHeartBitAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;
            right.SweepingSettings.Enabled = !left.SweepingSettings.Enabled;
            right.HeartBitInterval = 3 * left.HeartBitInterval;

            Assert.False(Equals(right.SweepingSettings, left.SweepingSettings));
            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(right.HeartBitInterval, left.HeartBitInterval));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfPolishingAndCleaningSettingsAndHeartBitAreDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;
            right.CleaningSettings.Enabled = !left.CleaningSettings.Enabled;
            right.HeartBitInterval = 3 * left.HeartBitInterval;

            Assert.False(Equals(right.CleaningSettings, left.CleaningSettings));
            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(right.HeartBitInterval, left.HeartBitInterval));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfEverythingExceptHeartBitIsDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;
            right.CleaningSettings.Enabled = !left.CleaningSettings.Enabled;
            right.SweepingSettings.Duration = 2 * left.SweepingSettings.Duration;

            Assert.False(Equals(right.CleaningSettings, left.CleaningSettings));
            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(right.SweepingSettings, left.SweepingSettings));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfEverythingIsDifferent()
        {
            var (left, right) = GetNewStandardBrushingSettingsInstances();
            right.PolishingSettings.Repeats = left.PolishingSettings.Repeats - 2;
            right.CleaningSettings.Enabled = !left.CleaningSettings.Enabled;
            right.SweepingSettings.Duration = 2 * left.SweepingSettings.Duration;
            right.HeartBitInterval = 3 * left.HeartBitInterval;

            Assert.False(Equals(right.CleaningSettings, left.CleaningSettings));
            Assert.False(Equals(right.PolishingSettings, left.PolishingSettings));
            Assert.False(Equals(right.SweepingSettings, left.SweepingSettings));
            Assert.False(Equals(right.HeartBitInterval, left.HeartBitInterval));
            Assert.False(Equals(left, right));
        }


        [Fact]
        public void NotAcceptZeroHeartBitIntervals()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new BrushingSettings()
                {
                    HeartBitInterval = TimeSpan.Zero
                };
            });
        }

        private (BrushingSettings, BrushingSettings) GetNewStandardBrushingSettingsInstances()
        {
            return (new BrushingSettings()
            {
                SweepingSettings =
                {
                    Enabled = true,
                    Duration = TimeSpan.FromMilliseconds(5000),
                    Repeats = 12
                },

                CleaningSettings =
                {
                    Enabled = true,
                    Duration = TimeSpan.FromMilliseconds(5000),
                    Repeats = 18
                },

                PolishingSettings =
                {
                    Enabled = true,
                    Duration = TimeSpan.FromMilliseconds(5000),
                    Repeats = 3
                },

                HeartBitInterval = TimeSpan.FromMilliseconds(1000)

            }, new BrushingSettings()
            {
                SweepingSettings =
                {
                    Enabled = true,
                    Duration = TimeSpan.FromMilliseconds(5000),
                    Repeats = 12
                },

                CleaningSettings =
                {
                    Enabled = true,
                    Duration = TimeSpan.FromMilliseconds(5000),
                    Repeats = 18
                },

                PolishingSettings =
                {
                    Enabled = true,
                    Duration = TimeSpan.FromMilliseconds(5000),
                    Repeats = 3
                },

                HeartBitInterval = TimeSpan.FromMilliseconds(1000)
            });
        }
    }
}