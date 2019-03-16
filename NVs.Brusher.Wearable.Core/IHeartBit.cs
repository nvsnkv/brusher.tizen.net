using System;

namespace NVs.Brusher.Wearable.Core
{
    public interface IHeartBit
    {
        void Start();
        void Stop();
        event EventHandler Tick;
    }
}