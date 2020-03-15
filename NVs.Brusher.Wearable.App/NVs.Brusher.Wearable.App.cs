using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace NVs.Brusher.Wearable.App
{
    class BrusherApp : NUIApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
            Window.Instance.KeyEvent += OnKeyEvent;

            var counterLayer = Window.Instance.GetDefaultLayer();
            var background = new ImageView(DirectoryInfo.Resource + "toothbrush-240.png")
            {
                PreMultipliedAlpha = true,
                PositionX = 60f,
                PositionY = 20f
            };
            counterLayer.Add(background);
        }

        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
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
    }
}
