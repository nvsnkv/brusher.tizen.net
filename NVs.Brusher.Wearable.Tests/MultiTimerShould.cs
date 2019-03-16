using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NVs.Brusher.Wearable.Core;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public class MultiTimerShould
    {
        [Theory]
        [InlineData(new[] { 1 }, new[] { TimeoutKind.First })]
        [InlineData(new[] { 1, 2 }, new[] { TimeoutKind.First, TimeoutKind.Last })]
        [InlineData(new[] { 1, 2, 3 }, new[] { TimeoutKind.First, TimeoutKind.Subsequent, TimeoutKind.Last })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { TimeoutKind.First, TimeoutKind.Subsequent, TimeoutKind.Subsequent, TimeoutKind.Last })]
        public async void RaiseProperTimeouts(int[] intervals, TimeoutKind[] expectedKinds)
        {
            var precision = 100;
            var actualKinds = new List<TimeoutKind>();
            var multiTimer = new MultiTimer(new HeartBit(precision), intervals);
            multiTimer.TimeoutOccured += (o, e) => actualKinds.Add(e);

            multiTimer.Start();
            Assert.Equal(TimerState.Running, multiTimer.State);

            await Task.Delay(intervals.Select(x => (x + 1) * precision).Sum() + precision);
            Assert.Equal(TimerState.Stopped, multiTimer.State);

            string Contact<T>(IEnumerable<T> source)
            {
                return source.Aggregate(new StringBuilder(), (sb, x) => sb.AppendFormat("{0} ", x)).ToString();
            }

            Assert.Equal(Contact(expectedKinds), Contact(actualKinds));
        }

        [Theory]
        [InlineData(new[] {5, 5, 3}, 16)]
        public async void CountRemainingTimeCorrectly(int[] intervals, long expectedRemaining)
        {
            var precision = 100;
            var multiTimer = new MultiTimer(new HeartBit(precision), intervals);
            Assert.Null(multiTimer.RemainingTicks);
            var evt = new ManualResetEvent(false);
            Exception ex = null;
            multiTimer.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(multiTimer.RemainingTicks) && multiTimer.State != TimerState.Stopped)
                {
                    try
                    {
                        Assert.True(multiTimer.RemainingTicks >= 0);
                        Assert.Equal(expectedRemaining--, multiTimer.RemainingTicks);
                        Assert.Equal(TimerState.Running, multiTimer.State);
                    }
                    catch (Exception ae)
                    {
                        ex = ae;
                    }
                }
                else if (e.PropertyName == nameof(multiTimer.State))
                {
                    if (multiTimer.State == TimerState.Stopped)
                    {
                        Assert.Equal(0, multiTimer.RemainingTicks);
                        evt.Set();
                    }
                }
            };

            multiTimer.Start();
            var result = evt.WaitOne(TimeSpan.FromMilliseconds(precision * (expectedRemaining + 1) * 10));
            if (ex != null)
            {
                throw ex;
            }

            Assert.Null(ex);
            Assert.True(result); 
            await Task.Delay(precision);

            Assert.Null(multiTimer.RemainingTicks);
            Assert.Equal(-1, expectedRemaining);
        }
    }
}