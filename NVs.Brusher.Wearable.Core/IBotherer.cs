using System.Threading.Tasks;

namespace NVs.Brusher.Wearable.Core
{
    public interface IBotherer
    {
        Task Bother();

        Task Disturb();
    }
}