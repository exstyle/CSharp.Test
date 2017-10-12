using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Certification.ManageFlow
{
    // The System.Threading.Tasks namespace also contains another class that can be used for parallel processing.
    // The Parallel class has a couple of static methods—For, ForEach, and Invoke—that you can use to parallelize work.

    // Parallelism involves taking a certain task and splitting it into a set of related tasks that can be executed concurrently.

    // Increasing performance with parallel processing happens only when you have a lot of 
    // work to be done that can be executed in parallel.For smaller work sets or for work that has to synchronize access to resources,
    //using the Parallel class can hurt performance.

    public static class PartParallelTask
    {
        public static void Run()
        {
            Exec2();
        }

        #region  1 - Parallel task

        public static void Exec1()
        {
            Parallel.For(0, 10, i =>
            {
                Thread.Sleep(1000);
            });

            var numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, i =>
            {
                Thread.Sleep(1000);
            });
        }

        #endregion

        #region  2 - Break or stop a parallel task

        // Break will ensure that the itération is finish, where as Stop will terminante
        public static void Exec2()
        {
            ParallelLoopResult result = Parallel.For(0, 1000, 
                (int i, ParallelLoopState loopState) =>
                    {
                        if (i == 500)
                        {
                            Trace.WriteLine("Breaking loop");
                            loopState.Break();
                        }
                    return;
                });
            
            ParallelLoopResult resultStop = Parallel.For(0, 1000,
               (int i, ParallelLoopState loopState) =>
               {
                   if (i == 500)
                   {
                       Trace.WriteLine("Stoping loop");
                       loopState.Stop();
                   }
                   return;
               });

            Trace.WriteLine($"The loop result is complete : {result.IsCompleted} ");
            Trace.WriteLine($"The loop resultStop is complete : {resultStop.IsCompleted}");

            Trace.WriteLine($"The loop result last Iteration number  : {result.LowestBreakIteration}"); // 500
            Trace.WriteLine($"The loop resultStop last Iteration number  : {resultStop.LowestBreakIteration}"); // Null

        }

        #endregion
    }
}