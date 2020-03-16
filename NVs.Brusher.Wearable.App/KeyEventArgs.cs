using Tizen.NUI;

namespace NVs.Brusher.Wearable.App
{
    class KeyEventArgs : Window.KeyEventArgs
    {
        public KeyEventArgs(Window.KeyEventArgs args)
        {
            Key = args.Key;
        }
        public bool Handled { get; set; }
    }
}