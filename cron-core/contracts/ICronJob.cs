using System;

namespace cron_core.contracts
{
    public interface ICronJob
    {
        void execute(DateTime date_time);
        void abort();
    }
}