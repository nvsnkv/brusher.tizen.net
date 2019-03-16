using System;
using System.Globalization;
using NVs.Brusher.Wearable.Core;
using Xamarin.Forms;

namespace NVs.Brusher.Wearable.App.Converters
{
    public sealed class TimerStateToObjectConverter<T> : IValueConverter
    {
        public T TrueObject { get; set; }

        public T FalseObject { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimerState))
            {
                return FalseObject;
            }

            var state = (TimerState)value;
            TimerState expectedState;

            if (parameter is TimerState)
            {
                expectedState = (TimerState) value;
            }
            else if (parameter is string )
            {
                if (!Enum.TryParse((string) parameter, out expectedState))
                {
                    return FalseObject;
                }
            }
            else
            {
                return FalseObject;
            }

            return expectedState == state ? TrueObject : FalseObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}