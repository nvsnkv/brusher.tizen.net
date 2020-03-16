using System;
using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.UIComponents;

namespace NVs.Brusher.Wearable.App.Views
{
    class TimerView : ITimerView
    {
        public TimerView(Layer counterLayer, DirectoryInfo directoryInfo)
        {
            var background = new ImageView(directoryInfo.Resource + "toothbrush-240.png")
            {
                PreMultipliedAlpha = true,
                SizeWidth = 240f,
                SizeHeight = 240f,
                PositionX = 60f,
                PositionY = 20f
            };

            var button = new PushButton()
            {
                PositionY = 285f,
                LabelText = "Start",
                LabelPadding = new Vector4(-27,0,0,0)
            };

            var view = new View();
            view.Add(background);
            view.Add(button);

            counterLayer.Add(view);

        }

        public event EventHandler ToggleStateButtonPressed;

        protected virtual void RaiseToggleButtonPressed()
        {
            ToggleStateButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}