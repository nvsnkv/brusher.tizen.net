using Tizen.Wearable.CircularUI.Forms.Renderer;
using Xamarin.Forms.Platform.Tizen;

namespace NVs.Brusher.Wearable.App
{
    class Program : FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            LoadApplication(new App());
        }

        static void Main(string[] args)
        {
            var app = new Program();
            Forms.Init(app);
            FormsCircularUI.Init();
            app.Run(args);
        }
    }
}
