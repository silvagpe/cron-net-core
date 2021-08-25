using System.Threading;

namespace cron_core.contracts
{
    public interface ICronDaemon
    {
        void AddJob(string schedule, ThreadStart action);
        void Start();
        void Stop();
    }
}