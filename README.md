# Simple Multi jobs cron .net

This project was refactored from https://github.com/kevincolyar/CronNET

Simple Cron .net is a simple C# library for running tasks based on a cron schedule.

## Cron Schedules

Supports most cron scheduling.  See tests for supported formats.

```
*    *    *    *    *  
┬    ┬    ┬    ┬    ┬
│    │    │    │    │
│    │    │    │    │
│    │    │    │    └───── day of week (0 - 6) (Sunday=0 )
│    │    │    └────────── month (1 - 12)
│    │    └─────────────── day of month (1 - 31)
│    └──────────────────── hour (0 - 23)
└───────────────────────── min (0 - 59)
```

```
  `* * * * *`        Every minute.
  `0 * * * *`        Top of every hour.
  `0,1,2 * * * *`    Every hour at minutes 0, 1, and 2.
  `*/2 * * * *`      Every two minutes.
  `1-55 * * * *`     Every minute through the 55th minute.
  `* 1,10,20 * * *`  Every 1st, 10th, and 20th hours.
```


## Console Example

```C#
class Program
{
    private static AutoResetEvent waitHandle = new AutoResetEvent(false);

    private static readonly CronDaemon cron_daemon = new CronDaemon();

    public static int count = 0;

    static void Main(string[] args)
    {
        //With param, every minute
        //-------------------------------------
        string value = "test";
        cron_daemon.AddJob(
            Guid.NewGuid(),
            "* * * * *",                
            param: value,
            (sender, e) =>
            {
                Console.WriteLine("Job 1. {0} - {1}", e.Argument, DateTime.Now);
            });

        //With ID, every two minutes
        //-------------------------------------
        Guid job2_Id = Guid.NewGuid();
        cron_daemon.AddJob(
            job2_Id,
            "*/2 * * * *",
            (sender, e) =>
            {
                Console.WriteLine("Job 2. {0} - {1}", DateTime.Now);
            });
        
        //at 17:28
        //-------------------------------------
        cron_daemon.AddJob(
            Guid.NewGuid(),
            "28 17 * * *",
            param: "JobValue",
            (sender, e) =>
            {
                Console.WriteLine("Job 17:28. {0} - {1}", e.Argument, DateTime.Now);
            });
        
        //Start
        //-------------------------------------
        cron_daemon.Start();
        Console.WriteLine("Stated {0}", DateTime.Now);

        //Remove job 2
        //-------------------------------------
        Thread.Sleep(TimeSpan.FromMinutes(3));
        cron_daemon.RemoveJob(job2_Id);
        Console.WriteLine("Job 2 - {0} removed", job2_Id);

        //New Job
        //-------------------------------------
        Thread.Sleep(TimeSpan.FromSeconds(20));
        cron_daemon.AddJob(
            Guid.NewGuid(),
            "* * * * *",
            param: "New job",
            (sender, e) =>
            {
                Console.WriteLine("Job 3. {0} - {1}", e.Argument, DateTime.Now);
            });

        Console.WriteLine("Job 3 added ", job2_Id);


        //Finish
        //-------------------------------------
        Thread.Sleep(TimeSpan.FromMinutes(3));
        cron_daemon.Stop();
        Console.WriteLine("Stoped {0}", DateTime.Now);
                    
        //-------------------------------------
        Console.CancelKeyPress += (o, e) =>
        {
            Console.WriteLine("Exit...");                
            waitHandle.Set();
        };

        //Wait for CancelKeyPress event
        waitHandle.WaitOne();

    }
}
```