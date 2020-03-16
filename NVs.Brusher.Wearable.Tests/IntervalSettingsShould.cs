using System;
using System.Collections;
using System.Collections.Generic;
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

            var _ = new StageSettings()
            {
                Enabled = @new.Enabled,
                Duration = @new.Duration,
                Repeats = @new.Repeats
            };
        }

        [Theory]
        [ClassData(typeof(DifferentIntervalDataGenerator))]
        public void BeDifferentIfValuesAreDifferent(bool enabledDiffers, bool durationDiffers, bool repeatsDiffers)
        {
            var (left, right) = GetNewIntervalSettingsInstances();
            if (enabledDiffers) left.Enabled = !right.Enabled;
            if (durationDiffers) left.Duration = 2 * right.Duration;
            if (repeatsDiffers) left.Repeats = 2 * right.Repeats;
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

    public sealed class DifferentIntervalDataGenerator : IEnumerable<object[]>
    {
        private class DiffersGenerator : IEnumerator<object[]>
        {
            private int current = 0;

            public bool MoveNext()
            {
                if (current == 8) { return false; }

                current++;
                Current = new object[]
                {
                    (current & 1) == 1, (current & 2) == 2, (current & 4) == 4
                };

                if (!((bool)Current[0] || (bool)Current[1] || (bool)Current[2]))
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
    }
}