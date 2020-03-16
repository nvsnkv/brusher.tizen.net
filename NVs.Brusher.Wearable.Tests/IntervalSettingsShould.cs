using System;
using NVs.Brusher.Wearable.Core.Settings;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class StageSettingsShould
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
            right.Duration = 2 * left.Duration;

            Assert.False(left.Duration.Equals(right.Duration));
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
            right.Duration = 2 * left.Duration;

            Assert.False(left.Duration.Equals(right.Duration));
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
            right.Duration = 2 * left.Duration;

            Assert.False(left.Enabled.Equals(right.Enabled));
            Assert.False(left.Duration.Equals(right.Duration));
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
                new StageSettings()
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
                new StageSettings()
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
                new StageSettings()
                {
                    Duration = TimeSpan.Zero
                };
            });
        }

        [Fact]
        public void BeValidByDefault()
        {
            var @new = new StageSettings();

            var test = new StageSettings()
            {
                Enabled = @new.Enabled,
                Duration = @new.Duration,
                Repeats = @new.Repeats
            };
        }

        private (StageSettings, StageSettings) GetNewIntervalSettingsInstances()
        {
            return (new StageSettings()
            {
                Enabled = true,
                Duration = TimeSpan.FromMinutes(1),
                Repeats = 1
            },
            new StageSettings()
            {
                Enabled = true,
                Duration = TimeSpan.FromMinutes(1),
                Repeats = 1
            });
        }
    }
}