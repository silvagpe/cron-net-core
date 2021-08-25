using System;

namespace cron_core.contracts
{
    public interface ICronJob
    {
        public Guid Id { get; set; }
        void execute(DateTime date_time);
        void Abort();
    }
}