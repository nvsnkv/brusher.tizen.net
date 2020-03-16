using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer;
using Tizen;
using Xunit;
using Xunit.Sdk;

namespace NVs.Brusher.Wearable.Tests
{
    public sealed class Logging_BrushingTimerShould
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

        [Fact(Skip = "Unstable")]
        public void LogsWarningWhenStartRequestedFromDifferentThreadsSimultaneously()
        {
            var logger = new TestLogger();

            var barrier = new ManualResetEventSlim();

            var timer = new BrushingTimer(Notificator, logger);

            var task = Enumerable.Repeat<Action>(() =>
            {
                barrier.Wait();
                timer.Start();
            }, 3).Select(Task.Run).ToArray();

            barrier.Set();
            Task.WaitAll(task);
            
            AssertRecordExists(logger.Messages, LogLevel.Warning, "Multiple threads attempted to start timer. This one lose");
        }

        [Fact]
        public void LogsPause()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger);
            timer.Start();
            timer.Pause();

            AssertRecordExists(logger.Messages, LogLevel.Debug, "Paused");
        }

        [Fact]
        public void LogsWarningWhenPauseCalledOnPausedTimer()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger);
            timer.Start();
            timer.Pause();
            timer.Pause();

            AssertRecordExists(logger.Messages, LogLevel.Warning, "Attempt made to pause timer which is not running");
        }

        [Fact]
        public void LogsWarningWhenPauseCalledOnStoppedTimer()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger);
            timer.Pause();

            AssertRecordExists(logger.Messages, LogLevel.Warning, "Attempt made to pause timer which is not running");
        }

        [Fact]
        public void LogsStop()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger);
            timer.Start();
            timer.Stop();

            AssertRecordExists(logger.Messages, LogLevel.Debug, "Stopped");
        }

        [Fact(Skip = "Unstable")]
        public void LogsWarningWhenStopRequestedFromDifferentThreadsSimultaneously()
        {
            var logger = new TestLogger();

            var barrier = new ManualResetEventSlim();

            var timer = new BrushingTimer(Notificator, logger);
            timer.Start();

            var task = Enumerable.Repeat<Action>(() =>
            {
                barrier.Wait();
                timer.Stop();
            }, 3).Select(Task.Run).ToArray();

            barrier.Set();
            Task.WaitAll(task);

            AssertRecordExists(logger.Messages, LogLevel.Warning, "Multiple threads attempted to stop timer. This one lose");
        }

        [Fact]
        public async Task LogsIfHeartBitHappenedForPausedTimer()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger).WithSettings(new BrushingSettings(){CleaningSettings = { Duration = TimeSpan.FromMilliseconds(300), Enabled = true, Repeats = 1}, HeartBitInterval = TimeSpan.FromMilliseconds(100)});
            timer.Start();
            timer.Pause();

            await Task.Delay(timer.Settings.HeartBitInterval * 5);
            timer.Stop();
            AssertRecordExists(logger.Messages, LogLevel.Debug, "HeartBit occured for paused timer.No action was performed");
        }

        [Fact]
        public async Task LogsWhenScheduleEnds()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger).WithSettings(new BrushingSettings() { CleaningSettings = { Duration = TimeSpan.FromMilliseconds(300), Enabled = true, Repeats = 1 }, HeartBitInterval = TimeSpan.FromMilliseconds(100) });
            timer.Start();

            await Task.Delay(timer.RemainingDuration.Value + 2 * timer.Settings.HeartBitInterval);
            AssertRecordExists(logger.Messages, LogLevel.Debug, "Schedule ended, stopping the timer");
        }

        [Fact]
        public async Task LogsWhenHeartbitPayloadGetsExecutedOnPausedTimer()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger).WithSettings(new BrushingSettings() { CleaningSettings = { Duration = TimeSpan.FromMilliseconds(300), Enabled = true, Repeats = 1 }, HeartBitInterval = TimeSpan.FromMilliseconds(100) });
            timer.Start();
            timer.Pause();

            await Task.Delay(timer.RemainingDuration.Value);
            timer.Stop();

            AssertRecordExists(logger.Messages, LogLevel.Debug, "HeartBit occured for paused timer.No action was performed");
        }

        [Fact]
        public async Task LogsWhenHeartBitPayloadGetsExecuted()
        {
            var logger = new TestLogger();

            var timer = new BrushingTimer(Notificator, logger).WithSettings(new BrushingSettings() { CleaningSettings = { Duration = TimeSpan.FromMilliseconds(300), Enabled = true, Repeats = 1 }, HeartBitInterval = TimeSpan.FromMilliseconds(100) });
            timer.Start();

            await Task.Delay(2 * timer.Settings.HeartBitInterval);
            timer.Stop();

            AssertRecordExists(logger.Messages, LogLevel.Debug, "HeartBit payload executed");
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