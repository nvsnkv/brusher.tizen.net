using System;
using NVs.Brusher.Wearable.Core.Settings;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class IntervalSettingsShould
    {
        [Fact]
        public void BeEqualIfTheyAreEqual()
        {
            var (left, right) = GetNewIntervalSettingsInstances();

            Assert.True(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfEnabledDiffers()
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            right.Enabled = !left.Enabled;

            Assert.False(left.Enabled.Equals(right.Enabled));
            Assert.False(Equals(left, right));

        }

        [Fact]
        public void BeDifferentIfDelayDiffers()
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            right.Delay = 2 * left.Delay;

            Assert.False(left.Delay.Equals(right.Delay));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfRepeatsDiffers()
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            right.Repeats = 2 * left.Repeats;

            Assert.False(left.Repeats.Equals(right.Repeats));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfRepeatsAndDelayDiffers()
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            right.Repeats = 2 * left.Repeats;
            right.Delay = 2 * left.Delay;

            Assert.False(left.Delay.Equals(right.Delay));
            Assert.False(left.Repeats.Equals(right.Repeats));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfRepeatsAndEnabledDiffers()
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            right.Repeats = 2 * left.Repeats;
            right.Enabled = !left.Enabled;

            Assert.False(left.Enabled.Equals(right.Enabled));
            Assert.False(left.Repeats.Equals(right.Repeats));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void BeDifferentIfEverythingDiffers()
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            right.Repeats = 2 * left.Repeats;
            right.Enabled = !left.Enabled;
            right.Delay = 2 * left.Delay;

            Assert.False(left.Enabled.Equals(right.Enabled));
            Assert.False(left.Delay.Equals(right.Delay));
            Assert.False(left.Repeats.Equals(right.Repeats));
            Assert.False(Equals(left, right));
        }

        [Fact]
        public void ReturnSameHashCodeIfValuesAreEqual()
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            Assert.True(left.GetHashCode() == right.GetHashCode());
        }

        [Fact]
        public void NotAcceptNegativeRepeatsCount()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new IntervalSettings()
                {
                    Repeats = -1
                };
            });
        }

        [Fact]
        public void NotAcceptZeroRepeatsCount()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new IntervalSettings()
                {
                    Repeats = 0
                };
            });
        }

        [Fact]
        public void NotAcceptZeroDelay()
        {
            Assert.Equal(TimeSpan.Zero, TimeSpan.FromSeconds(0));
            Assert.Throws<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new IntervalSettings()
                {
                    Delay = TimeSpan.Zero
                };
            });
        }

        [Fact]
        public void BeValidByDefault()
        {
            var @new = new IntervalSettings();

            var test = new IntervalSettings()
            {
                Enabled = @new.Enabled,
                Delay = @new.Delay,
                Repeats = @new.Repeats
            };
        }

        private (IntervalSettings, IntervalSettings) GetNewIntervalSettingsInstances()
        {
            return (new IntervalSettings()
            {
                Enabled = true,
                Delay = TimeSpan.FromMinutes(1),
                Repeats = 1
            },
            new IntervalSettings()
            {
                Enabled = true,
                Delay = TimeSpan.FromMinutes(1),
                Repeats = 1
            });
        }
    }
}