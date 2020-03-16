using System;
using Microsoft.Extensions.Logging;
using NVs.Brusher.Wearable.App.Controllers;
using NVs.Brusher.Wearable.App.Logging;
using NVs.Brusher.Wearable.App.Views;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer;
using Tizen.NUI;
using Tizen.System;

namespace NVs.Brusher.Wearable.App
{
    sealed class BrusherApp : NUIApplication
    {
        private ILogger appLogger;
        protected override void OnCreate()
        {
            appLogger = new LogWrapper<BrusherApp>(LogLevel.Information);
            try
            {
                base.OnCreate();
                Initialize();
            }
            catch (Exception e)
            {
                appLogger.LogCritical(e, "Uhnandled exception in OnCreate, terminating applicaiton");
                throw;
            }
        }

        void Initialize()
        {
            var vibroLogger = new LogWrapper<VibroNotificator>(LogLevel.Debug);
            var timerLogger = new LogWrapper<BrushingTimer>(LogLevel.Debug);

            var brushingTimer =
                new BrushingTimer(new VibroNotificator(Vibrator.Vibrators[0], vibroLogger), timerLogger)
                    .WithSettings(BrushingSettings.Default);
            brushingTimer.AsyncExceptionRaised += (o, e) =>
            {
                timerLogger.LogError(e.Exception, "Asynchronous exception happened");
            };

            Window.Instance.KeyEvent += OnBaseKeyEvent;

            var timerView = new TimerView(Window.Instance.GetDefaultLayer(), DirectoryInfo, brushingTimer);
            var timerViewController = new TimerViewController(timerView, brushingTimer);

            this.KeyEvent += timerViewController.HandleKeyEvent;
            this.KeyEvent += OnKeyEvent;
        }

        private event EventHandler<KeyEventArgs> KeyEvent;

        void OnBaseKeyEvent(object sender, Window.KeyEventArgs e)
        {
            RaiseKeyEvent(new KeyEventArgs(e));
        }

        private void OnKeyEvent(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Handled) return;

                if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
                {
                    appLogger.LogInformation("Escape key pressed, exiting...");
                    Exit();
                }
            }
            catch (Exception ex)
            {
                appLogger.LogError(ex, "Error occured while processing KeyEvent");
            }
        }

        static void Main(string[] args)
        {
            var app = new BrusherApp();
            app.Run(args);
        }

        private void RaiseKeyEvent(KeyEventArgs e)
        {
            try
            {
                KeyEvent?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                appLogger.LogError(ex, "Error occured while raising KeyEvent");
            }
        }
    }
}
