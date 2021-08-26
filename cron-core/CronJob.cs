using System;
using System.ComponentModel;
using System.Threading;
using cron_core.contracts;

namespace cron_core
{
    public class CronJob : ICronJob
    {
        private readonly ICronSchedule _cron_schedule = new CronSchedule();
        
        private BackgroundWorker backgroundWorker;
        private object _param;

        public Guid Id { get; set; }
    

        public CronJob(Guid id, string schedule, object param, DoWorkEventHandler doWork)
        {
            _cron_schedule = new CronSchedule(schedule);
            _param = param;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork+= doWork;            

        }

        private object _lock = new object();
        public void Execute(DateTime date_time)
        {
            lock (_lock)
            {
                if (!_cron_schedule.isTime(date_time))
                    return;

                if (backgroundWorker.IsBusy == true)
                    return;

                backgroundWorker.RunWorkerAsync(this._param);
            }
        }

        public void Abort()
        {
            if (backgroundWorker.IsBusy)
                backgroundWorker.CancelAsync();
            
        }

    }
}