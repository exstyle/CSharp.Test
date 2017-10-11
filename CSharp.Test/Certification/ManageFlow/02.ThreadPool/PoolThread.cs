using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Certification.ManageFlow
{
    /// <summary>
    /// Méthode des thread.
    /// </summary>
    public static class PoolThread
    {
        public static void Run()
        {
            Exec1();
        }

        #region 1 - Queuing some work to the thread pool
        
        public static void Exec1()
        {
            ThreadPool.QueueUserWorkItem((s) =>
            {
                Trace.WriteLine("Working on a thread from threadpool");
            });

            Console.ReadLine();
        }

        #endregion
        
    }
}