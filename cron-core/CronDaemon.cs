using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;
using cron_core.contracts;

namespace cron_core
{
    public class CronDaemon : ICronDaemon
    {
        private readonly System.Timers.Timer timer = new System.Timers.Timer(30000);
        private readonly List<ICronJob> cron_jobs = new List<ICronJob>();
        private DateTime _last = DateTime.Now;

        public CronDaemon()
        {
            timer.AutoReset = true;
            timer.Elapsed += timer_elapsed;
        }

        public void AddJob(Guid id, string schedule, DoWorkEventHandler doWork)
        {
            AddJob(id, schedule, null, doWork);
        }

        public void AddJob(Guid id, string schedule, object param, DoWorkEventHandler doWork)
        {
            var cj = new CronJob(id, schedule, param, doWork);
            cron_jobs.Add(cj);
        }

        public void RemoveJob(Guid id)
        {
            var job = this.cron_jobs.FirstOrDefault(x => x.Id == id);

            if (job != null)
            {
                job.Abort();
                this.cron_jobs.Remove(job);
            }
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();

            foreach (CronJob job in cron_jobs)
                job.Abort();
        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute != _last.Minute)
            {
                _last = DateTime.Now;
                foreach (ICronJob job in cron_jobs)
                    job.execute(DateTime.Now);
            }
        }
    }
}