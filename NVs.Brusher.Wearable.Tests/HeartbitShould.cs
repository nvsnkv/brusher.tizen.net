using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NVs.Brusher.Wearable.Models;
using Xunit;

namespace NVs.Brusher.Wearable.Tests
{
    public class HeartbitShould    
    {
        [Fact]
        public void StartAndStopWithoutErrors()
        {
            var hb = new HeartBit(1000);
            hb.Start();
            hb.Stop();
        }

        [Fact]
        public async Task NotTicksWhenStopped()
        {
            var timeline = new List<long>();
            var stopwatch = new Stopwatch();
            var hb = new HeartBit(100);
            hb.Tick += (o,e) => timeline.Add(stopwatch.ElapsedMilliseconds);

            stopwatch.Start();
            hb.Start();
            await Task.Delay(300);
            hb.Stop();
            stopwatch.Stop();

            var maxTime = timeline.Max();
            Assert.True(maxTime < stopwatch.ElapsedMilliseconds);
        }

        [Fact]
        public async Task TickWithRequestedPrecision()
        {
            var precision = 100;
            var duration = 500;

            var expectedTicksCount = duration / precision;
        }
    }
}
