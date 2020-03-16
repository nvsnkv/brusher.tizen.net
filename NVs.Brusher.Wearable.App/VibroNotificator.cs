using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NVs.Brusher.Wearable.Core.Timer;
using Tizen.System;

namespace NVs.Brusher.Wearable.App
{
    class VibroNotificator : INotificator
    {
        private readonly Vibrator vibrator;
        private readonly ILogger<INotificator> logger;

        public VibroNotificator(Vibrator vibrator, ILogger<INotificator> logger)
        {
            this.vibrator = vibrator;
            this.logger = logger;
        }

        public void NotifyTimerFinished()
        {
            logger.LogTrace("Requested TimerFinished notification");
            var task = Task.Run(async () =>
            {
                vibrator.Vibrate(100, 100);
                await Task.Delay(200);
                vibrator.Vibrate(100, 100);
                await Task.Delay(200);
                vibrator.Vibrate(300, 100);
            });

            task.Wait();

            if (task.Exception != null)
            {
                throw task.Exception;
            }

            logger.LogDebug("Notified that timer finished");
        }

        public void NotifyStageChanged()
        {
            logger.LogTrace("Requested StageChanged notification");
            vibrator.Vibrate(100, 100);
            Task.Delay(100).Wait();
            logger.LogDebug("Notified that state has changed");

        }

        public void NotifyTimerStarted()
        {
            logger.LogTrace("Requested TimerStarted notification");
            var task = Task.Run(async () =>
            {
                vibrator.Vibrate(100, 100);
                await Task.Delay(200);
                vibrator.Vibrate(100, 100);
            });

            task.Wait();

            if (task.Exception != null)
            {
                throw task.Exception;
            }

            logger.LogDebug("Notified that timer started");
        }
    }
}