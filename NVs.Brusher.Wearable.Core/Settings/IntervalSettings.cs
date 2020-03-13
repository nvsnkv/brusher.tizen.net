using System;

namespace NVs.Brusher.Wearable.Core.Settings
{
    public sealed class IntervalSettings
    {
        private int repeats;
        private TimeSpan delay;

        public IntervalSettings()
        {
            delay = TimeSpan.FromMilliseconds(1000);
            repeats = 1;
        }

        private bool Equals(IntervalSettings other)
        {
            return Enabled == other.Enabled && Delay.Equals(other.Delay) && Repeats == other.Repeats;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is IntervalSettings other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Enabled.GetHashCode();
                hashCode = (hashCode * 397) ^ Delay.GetHashCode();
                hashCode = (hashCode * 397) ^ Repeats;
                return hashCode;
            }
        }

        public bool Enabled { get; set; }

        public TimeSpan Delay
        {
            get => delay;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentException("This property does not accept empty TimeSpans", nameof(Delay));
                }
                delay = value;
            }
        }

        public int Repeats
        {
            get => repeats;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("This property does not accept negative values", nameof(Repeats));
                }
                repeats = value;
            }
        }
    }
}