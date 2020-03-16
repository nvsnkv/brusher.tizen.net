using System;
using NVs.Brusher.Wearable.App.Controllers;
using NVs.Brusher.Wearable.App.Views;
using NVs.Brusher.Wearable.Core.Settings;
using NVs.Brusher.Wearable.Core.Timer;
using Tizen.NUI;
using Tizen.System;

namespace NVs.Brusher.Wearable.App
{
    sealed class BrusherApp : NUIApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
            var brushingTimer = new BrushingTimer(new VibroNotificator(Vibrator.Vibrators[0])).WithSettings(BrushingSettings.Default);
            
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
            if (e.Handled) return;

            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                Exit();
            }
        }

        static void Main(string[] args)
        {
            var app = new BrusherApp();
            app.Run(args);
        }

        private void RaiseKeyEvent(KeyEventArgs e)
        {
            KeyEvent?.Invoke(this, e);
        }
    }
}
