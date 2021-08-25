using System;
using System.ComponentModel;
using System.Threading;
using cron_core.contracts;

namespace cron_core
{
    public class CronJob : ICronJob
    {
        private readonly ICronSchedule _cron_schedule = new CronSchedule();
        private readonly ThreadStart _thread_start;
        private Thread _thread;
        

        public CronJob(string schedule, ThreadStart thread_start)
        {
            _cron_schedule = new CronSchedule(schedule);
            _thread_start = thread_start;
            _thread = new Thread(thread_start);
            
        }

        private object _lock = new object();
        public void execute(DateTime date_time)
        {
            lock (_lock)
            {
                if (!_cron_schedule.isTime(date_time))
                    return;

                if (_thread.ThreadState == ThreadState.Running)
                    return;

                _thread = new Thread(_thread_start);
                _thread.Start();
            }
        }

        public void abort()
        {
            //TODO: Refactor
          //_thread.Abort();  
        }

    }
}