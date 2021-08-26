using System;

namespace cron_core.contracts
{
    public interface ICronJob
    {
        public Guid Id { get; set; }
        void Execute(DateTime date_time);
        void Abort();
    }
}