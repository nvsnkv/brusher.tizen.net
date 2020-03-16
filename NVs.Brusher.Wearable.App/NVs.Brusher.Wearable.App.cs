using System;
using NVs.Brusher.Wearable.App.Controllers;
using NVs.Brusher.Wearable.App.Views;
using Tizen.NUI;

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
            Window.Instance.KeyEvent += OnBaseKeyEvent;
            
            var timerView = new TimerView(Window.Instance.GetDefaultLayer(), DirectoryInfo);
            var timerViewController = new TimerViewController(timerView);

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
