using System.Threading.Tasks;

namespace NVs.Brusher.Wearable.Core.Timer
{
    public interface INotificator
    {
        void NotifyTimerFinished();
        void NotifyStageChanged();
        void NotifyTimerStarted();
    }
}