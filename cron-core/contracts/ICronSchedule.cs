using System;

namespace cron_core.contracts
{
    public interface ICronSchedule
    {
        bool isValid(string expression);
        bool isTime(DateTime date_time);
    }
}