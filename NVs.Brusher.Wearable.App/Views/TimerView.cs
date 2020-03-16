using System;
using System.ComponentModel;
using NVs.Brusher.Wearable.Core.Timer;
using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.UIComponents;

namespace NVs.Brusher.Wearable.App.Views
{
    class TimerView : ITimerView
    {
        private readonly BrushingTimer brushingTimer;
        private readonly PushButton button;

        private readonly Vector4 startPauseLabelPadding = new Vector4(-27f, 0,0,0);
        private readonly Vector4 coninueLabelPadding = new Vector4(-54f, 0, 0, 0);

        public TimerView(Layer counterLayer, DirectoryInfo directoryInfo, BrushingTimer brushingTimer)
        {
            this.brushingTimer = brushingTimer;
            var background = new ImageView(directoryInfo.Resource + "toothbrush-240.png")
            {
                PreMultipliedAlpha = true,
                SizeWidth = 240f,
                SizeHeight = 240f,
                PositionX = 60f,
                PositionY = 20f
            };

            button = new PushButton()
            {
                PositionY = 285f,
                LabelText = "Start",
                LabelPadding = new Vector4(-27,0,0,0)
            };

            button.Pressed += (o, e) => RaiseToggleButtonPressed();

            var view = new View();
            view.Add(background);
            view.Add(button);

            brushingTimer.PropertyChanged += TimerPropertyChanged;

            counterLayer.Add(view);
        }

        private void TimerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BrushingTimer.State):
                    UpdateButtonText();
                    break;

                case nameof(BrushingTimer.RemainingDuration):
                    UpdateRemainingDuration();
                    break;
            }
        }

        private void UpdateRemainingDuration()
        {
            
        }

        private void UpdateButtonText()
        {
            switch (brushingTimer.State)
            {
                case TimerState.Stopped:
                    button.LabelText = "Start";
                    button.LabelPadding = startPauseLabelPadding;
                    break;
                case TimerState.Paused:
                    button.LabelText = "Continue";
                    button.LabelPadding = coninueLabelPadding;
                    break;
                case TimerState.Running:
                    button.LabelText = "Pause";
                    button.LabelPadding = startPauseLabelPadding;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public event EventHandler ToggleStateButtonPressed;

        protected virtual bool RaiseToggleButtonPressed()
        {
            ToggleStateButtonPressed?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }
}