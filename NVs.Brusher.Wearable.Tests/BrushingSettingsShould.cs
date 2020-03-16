using System;
using System.Collections;
using System.Collections.Generic;
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
            var (left, right) = DifferentBrushingSettingsGenerator.GetNewStandardBrushingSettingsInstances();

            var result = left.Equals(right);
            Assert.True(result);
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

        [Fact]
        public void NotAcceptNegativeDelays()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new BrushingSettings()
                {
                    Delay = TimeSpan.FromMilliseconds(-1)
                };
            });
        }

        [Theory]
        [ClassData(typeof(DifferentBrushingSettingsGenerator))]
        public void BeDifferentIfDataDiffers(bool delayDiffers, bool heartBitDiffers, bool sweepDiffers, bool cleanDiffers, bool polishDiffers)
        {
            var (left, right) = DifferentBrushingSettingsGenerator.GetNewStandardBrushingSettingsInstances();

            if (delayDiffers) right.Delay = 2 * left.Delay;
            if (heartBitDiffers) right.HeartBitInterval = 2 * left.HeartBitInterval;
            if (sweepDiffers) right.SweepingSettings.Enabled = !left.SweepingSettings.Enabled;
            if (cleanDiffers) right.CleaningSettings.Enabled = !left.CleaningSettings.Enabled;
            if (polishDiffers) right.PolishingSettings.Enabled = !left.PolishingSettings.Enabled;

            Assert.False(left.Equals(right));
        }
    }

    public sealed class DifferentBrushingSettingsGenerator : IEnumerable<object[]>
    {
        private class DiffersGenerator : IEnumerator<object[]>
        {
            private int current = 0;

            public bool MoveNext()
            {
                if (current == 32) { return false; }

                current++;
                Current = new object[]
                {
                    (current & 1) == 1, (current & 2) == 2, (current & 4) == 4, (current & 8) == 8, (current & 16) == 16
                };

                if (!((bool)Current[0] || (bool)Current[1] || (bool)Current[2] || (bool)Current[3] || (bool)Current[4]))
                {
                    return MoveNext();
                }

                return true;

            }

            public void Reset()
            {
                current = 0;
            }

            public object[] Current { get; private set; } = { true, false, false, false, false };

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return new DiffersGenerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static (BrushingSettings, BrushingSettings) GetNewStandardBrushingSettingsInstances()
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

                HeartBitInterval = TimeSpan.FromMilliseconds(1000),
                Delay = TimeSpan.FromMilliseconds(300)

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

                HeartBitInterval = TimeSpan.FromMilliseconds(1000),
                Delay =  TimeSpan.FromMilliseconds(300)
            });
        }
    }
}