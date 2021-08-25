using System;
using System.Threading;
using cron_core;

namespace cron_console_test
{
    class Program
    {
        private static AutoResetEvent waitHandle = new AutoResetEvent(false);

        private static readonly CronDaemon cron_daemon = new CronDaemon();

        public static int count = 0;

        static void Main(string[] args)
        {
            string valor = "teste";
            cron_daemon.AddJob(
                Guid.NewGuid(),
                "* * * * *",
                (sender, e) =>
                {
                    Console.WriteLine("Job 1. {0} - {1}", e.Argument, DateTime.Now);
                },
                param: valor);

            Guid idJob2 = Guid.NewGuid();

            cron_daemon.AddJob(
                idJob2,
                "*/2 * * * *",
                (sender, e) =>
                {
                    Console.WriteLine("Job 2. {0} - {1}", e.Argument, DateTime.Now);
                },
                param: valor);
            
            cron_daemon.AddJob(
                Guid.NewGuid(),
                "28 17 * * *",
                (sender, e) =>
                {
                    Console.WriteLine("Job 17:28. {0} - {1}", e.Argument, DateTime.Now);
                },
                param: valor);
            
            cron_daemon.Start();
            Console.WriteLine("Stated {0}", DateTime.Now);

            Thread.Sleep(TimeSpan.FromMinutes(3));
            cron_daemon.RemoveJob(idJob2);
            Console.WriteLine("Job 2 - {0} removed", idJob2);

            Thread.Sleep(TimeSpan.FromSeconds(20));
            cron_daemon.AddJob(
                Guid.NewGuid(),
                "* * * * *",
                (sender, e) =>
                {
                    Console.WriteLine("Job 3. {0} - {1}", e.Argument, DateTime.Now);
                },
                param: valor);

            Console.WriteLine("Job 3 added ", idJob2);

            // cron_daemon.Stop();
            // Console.WriteLine("Stoped {0}", DateTime.Now);

            // Wait and sleep forever. Let the cron daemon run.
            //while(true) Thread.Sleep(6000);
            Console.CancelKeyPress += (o, e) =>
            {
                //Console.WriteLine("Saindo...");
                Console.WriteLine("Sistema finalizado...");

                // Libera a continuação da thread principal
                waitHandle.Set();
            };

            // Aguarda que o evento CancelKeyPress ocorra
            waitHandle.WaitOne();

        }

        static void task()
        {
            count++;
            Console.WriteLine("Hello, world. {0} - {1}", count, DateTime.Now);

        }
    }
}
