using System;
using NVs.Brusher.Wearable.App.ViewModels;
using NVs.Brusher.Wearable.App.Views;
using NVs.Brusher.Wearable.Core;
using NVs.Brusher.Wearable.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NVs.Brusher.Wearable.App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        private readonly int[] intervals =
        {
            3,
            4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4,
            4, 4, 4, 4, 4
        };

        public App()
        {
            InitializeComponent();

            var botherer = new Botherer(Tizen.System.Vibrator.Vibrators[0]);
            var timer = new MultiTimer(new HeartBit(1000), intervals);

            timer.TimeoutOccured += async (o, kind) =>
            {
                switch (kind)
                {
                    case TimeoutKind.First:
                        await botherer.Disturb();
                        break;
                    case TimeoutKind.Subsequent:
                        await botherer.Bother();
                        break;
                    case TimeoutKind.Last:
                        await botherer.Disturb();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unexpected timeout kind!");
                }
            };

            MainPage = new TimerView()
            {
                BindingContext = new TimerViewModel(timer)
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

