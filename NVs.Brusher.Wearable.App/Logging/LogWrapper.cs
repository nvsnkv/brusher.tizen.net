using System;
using Microsoft.Extensions.Logging;

namespace NVs.Brusher.Wearable.App.Logging
{
    public class LogWrapper<T> : ILogger<T>
    {
        private string TypeName => typeof(T).Name;

        private readonly LogLevel minimalLogLevel;
        private static string Tag { get; set; } = nameof(BrusherApp);
        
        public LogWrapper(LogLevel minimalLogLevel)
        {
            this.minimalLogLevel = minimalLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter != null ? formatter(state, exception) : state.ToString();

            switch (logLevel)
            {
                case LogLevel.Trace:
                    Tizen.Log.Verbose(Tag, $"{logLevel}: {TypeName}: {message}");
                    break;
                case LogLevel.Debug:
                    Tizen.Log.Debug(Tag, $"{logLevel}: {TypeName}: {message}");
                    break;
                case LogLevel.Information:
                    Tizen.Log.Info(Tag, $"{logLevel}: {TypeName}: {message}");
                    break;
                case LogLevel.Warning:
                    Tizen.Log.Warn(Tag, $"{logLevel}: {TypeName}: {message}");
                    break;
                case LogLevel.Error:
                    Tizen.Log.Error(Tag, $"{logLevel}: {TypeName}: {message}");
                    break;
                case LogLevel.Critical:
                    Tizen.Log.Fatal(Tag, $"{logLevel}: {TypeName}: {message}");
                    break;
                case LogLevel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= minimalLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}