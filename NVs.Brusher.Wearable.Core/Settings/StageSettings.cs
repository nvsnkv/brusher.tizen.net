using System;

namespace NVs.Brusher.Wearable.Core.Settings
{
    public sealed class StageSettings
    {
        private int repeats;
        private TimeSpan duration;

        public StageSettings()
        {
            duration = TimeSpan.FromMilliseconds(1000);
            repeats = 1;
        }

        private bool Equals(StageSettings other)
        {
            return Enabled == other.Enabled && Duration.Equals(other.Duration) && Repeats == other.Repeats;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is StageSettings other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Enabled.GetHashCode();
                hashCode = (hashCode * 397) ^ Duration.GetHashCode();
                hashCode = (hashCode * 397) ^ Repeats;
                return hashCode;
            }
        }

        public bool Enabled { get; set; }

        public TimeSpan Duration
        {
            get => duration;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentException("This property does not accept empty TimeSpans", nameof(Duration));
                }
                duration = value;
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