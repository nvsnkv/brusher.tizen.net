using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NVs.Brusher.Wearable.Core.Timer;
using Xunit;
using Xunit.Sdk;

namespace NVs.Brusher.Wearable.Tests
{
    public class Logging_BrushingTimerShould
    {
        private static readonly INotificator Notificator = new Mock<INotificator>().Object;

        private class TestLogger : ILogger<BrushingTimer>
        {
            public ConcurrentQueue<(LogLevel, string)> Messages { get; } = new ConcurrentQueue<(LogLevel, string)>();

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                string message;

                if (formatter != null)
                {
                    message = formatter(state, exception);
                }
                else
                {
                    message = state.ToString();
                }

                Messages.Enqueue((logLevel, message));
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel != LogLevel.None;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void LogItsCreation()
        {
            var logger = new TestLogger();

            var _ = new BrushingTimer(Notificator, logger);

            AssertRecordExists(logger.Messages, LogLevel.Debug, "Created");
        }

        [Fact]
        public void LogsStart()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger);
            timer.Start();

            AssertRecordExists(logger.Messages, LogLevel.Debug, "Started");
        }

        [Fact]
        public void LogsWarningWhenStartCalledOnRunningTimer()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger);
            timer.Start();
            timer.Start();

            AssertRecordExists(logger.Messages, LogLevel.Warning, "Attempt made to start already running instance");
        }

        [Fact]
        public void LogsWarningWhenStartRequestedFromDifferentThreadsSimultaneously()
        {
            var logger = new TestLogger();

            var barier = new ManualResetEventSlim();

            var timer = new BrushingTimer(Notificator, logger);

            var task = Enumerable.Repeat<Action>(() =>
            {
                barier.Wait();
                timer.Start();
            }, 3).Select(Task.Run).ToArray();

            barier.Set();
            Task.WaitAll(task);
            
            AssertRecordExists(logger.Messages, LogLevel.Warning, "Multiple threads attempted to start timer. This one lose");
        }

        private void AssertRecordExists(IEnumerable<(LogLevel, string)> messages, LogLevel level, string message)
        {
            Assert.Contains(messages, (m) =>
            {
                var (lvl, msg) = m;
                return level == lvl && message.Equals(msg, StringComparison.InvariantCulture);
            });
        }
    }
}