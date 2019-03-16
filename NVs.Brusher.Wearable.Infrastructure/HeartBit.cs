using System;
using System.Threading;
using System.Threading.Tasks;
using NVs.Brusher.Wearable.Core;

namespace NVs.Brusher.Wearable.Infrastructure
{
    public sealed class HeartBit : IHeartBit
    {
        private readonly object thisLock = new object();
        private readonly int precision;
        private CancellationTokenSource cts;

        private volatile bool isRunning;

        public HeartBit(int precision)
        {
            this.precision = precision;
        }

        public void Start()
        {
            if (isRunning) { return; }

            lock (thisLock)
            {
                if (isRunning) { return; }

                isRunning = true;
            }

            cts = new CancellationTokenSource();
            var token = cts.Token;
            Task.Run(async () =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    RaiseTick();
                    await Task.Delay(precision, token);
                }
            }, token);

        }

        public void Stop()
        {
            if (!isRunning) { return; }

            lock (thisLock)
            {
                if (!isRunning) { return; }

                isRunning = false;
            }

            cts.Cancel();
        }

        public event EventHandler Tick;

        private void RaiseTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}