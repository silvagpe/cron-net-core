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
            
            cron_daemon.AddJob("* * * * *", ()=>{
                Console.WriteLine("Job 1. {0}", DateTime.Now);
            });
            cron_daemon.AddJob("*/2 * * * *", ()=>{
                Console.WriteLine("Job 2. {0}", DateTime.Now);
            });
            cron_daemon.Start();

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
