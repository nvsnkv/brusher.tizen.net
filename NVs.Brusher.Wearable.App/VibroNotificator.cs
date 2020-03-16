using System.Threading.Tasks;
using NVs.Brusher.Wearable.Core.Timer;
using Tizen.System;

namespace NVs.Brusher.Wearable.App
{
    class VibroNotificator : INotificator
    {
        private readonly Vibrator vibrator;

        public VibroNotificator(Vibrator vibrator)
        {
            this.vibrator = vibrator;
        }

        public void NotifyTimerFinished()
        {
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
        }

        public void NotifyStageChanged()
        {
            vibrator.Vibrate(100, 100);
        }

        public void NotifyTimerStarted()
        {
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
        }
    }
}