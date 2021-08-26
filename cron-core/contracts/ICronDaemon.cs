using System;
using System.ComponentModel;
using System.Threading;

namespace cron_core.contracts
{
    public interface ICronDaemon
    {                
        void AddJob(Guid id, string schedule, object param, DoWorkEventHandler doWork);
        void AddJob(Guid id, string schedule, DoWorkEventHandler doWork);
        void RemoveJob(Guid id);
        void Start();
        void Stop();
    }
}