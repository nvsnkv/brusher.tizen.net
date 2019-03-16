using System.Threading.Tasks;
using NVs.Brusher.Wearable.Core;
using Tizen.System;

namespace NVs.Brusher.Wearable.Infrastructure
{
    public sealed class Botherer : IBotherer
    {
        private readonly int feedback = 75;
        private readonly Vibrator vibrator;

        public Botherer(Vibrator vibrator)
        {
            this.vibrator = vibrator;
        }

        public async Task Bother()
        {
            vibrator.Vibrate(200, feedback);
            await Task.Delay(300);
            vibrator.Vibrate(200, feedback);
            await Task.Delay(200);
        }

        public async Task Disturb()
        {
            vibrator.Vibrate(200, feedback);
            await Task.Delay(300);
            vibrator.Vibrate(200, feedback);
            await Task.Delay(300);
            vibrator.Vibrate(200, feedback);
            await Task.Delay(200);
        }
    }
}