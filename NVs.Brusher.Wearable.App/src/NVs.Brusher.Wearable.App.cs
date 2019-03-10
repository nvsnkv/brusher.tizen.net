using System.Runtime.CompilerServices;
using Tizen.Wearable.CircularUI.Forms.Renderer;
using Xamarin.Forms.Platform.Tizen;

[assembly:InternalsVisibleTo("NVs.Brusher.Wearable.Tests")]
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
