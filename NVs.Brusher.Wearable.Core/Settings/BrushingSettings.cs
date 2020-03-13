using System;
using System.Collections.Generic;

namespace NVs.Brusher.Wearable.Core.Settings
{
    public sealed class BrushingSettings
    {
        public BrushingSettings()
        {
            SweepingSettings = new IntervalSettings();
            CleaningSettings = new IntervalSettings();
            PolishingSettings = new IntervalSettings();
            HeartBitInterval = TimeSpan.FromMilliseconds(1000);
        }

        public IntervalSettings SweepingSettings { get; private set; }

        public IntervalSettings CleaningSettings { get; private set; }

        public IntervalSettings PolishingSettings { get; private set; }

        public TimeSpan HeartBitInterval
        {
            get => heartBitInterval;
            set
            {
                if (value <= TimeSpan.Zero) throw new ArgumentException("Interval provided is too short", nameof(HeartBitInterval));
                
                heartBitInterval = value;
            }
        }

        public static readonly BrushingSettings Default = new BrushingSettings()
        {
            SweepingSettings =
            {
                Enabled = true,
                Delay = TimeSpan.FromSeconds(5),
                Repeats = 12
            },

            CleaningSettings = {
                Enabled = true,
                Delay = TimeSpan.FromSeconds(5),
                Repeats = 18
            },

            PolishingSettings = {
                Enabled = true,
                Delay = TimeSpan.FromSeconds(5),
                Repeats = 3
            },
            
            HeartBitInterval = TimeSpan.FromSeconds(1)
        };

        private TimeSpan heartBitInterval;

        private bool Equals(BrushingSettings other)
        {
            return heartBitInterval.Equals(other.heartBitInterval) && Equals(SweepingSettings, other.SweepingSettings) && Equals(CleaningSettings, other.CleaningSettings) && Equals(PolishingSettings, other.PolishingSettings);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is BrushingSettings other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = heartBitInterval.GetHashCode();
                hashCode = (hashCode * 397) ^ (SweepingSettings != null ? SweepingSettings.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CleaningSettings != null ? CleaningSettings.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PolishingSettings != null ? PolishingSettings.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}