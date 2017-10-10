using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Chapter1
{
    /// <summary>
    /// Méthode des thread.
    /// </summary>
    public static class PartThread
    {
        public static void Run()
        {
            Exec3();
        }
        
        #region 1

        /// <summary>
        /// Illustration d'un appel de méthode sur un autre thread.
        /// Ex : Background et Foreground thread
        /// Ex : Join
        /// </summary>
        public static void Exec1()
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod1));

            // Foreground vs background.
            // If declare as background, as soon as the run method is over, the the other thread such as t will close.
            // So we might have only 1 or 0 Message ThreadProc
            // ! .join will wait for other thread to terminate...
            t.IsBackground = false;

            t.Start();

            for (int i = 0; i < 4; i++)
            {
                Trace.WriteLine("Mainthread: Dosomework.");
                Thread.Sleep(0);
            }

            //t.Join(); // Will wait for other Thread to terminate
        }

        public static void ThreadMethod1()
        {
            for (int i = 0; i < 10; i++)
            {
                Trace.WriteLine(string.Format("ThreadProc:{0}", i));
                Console.WriteLine("ThreadProc:{0}", i);
                Thread.Sleep(0);
            }

        }
        #endregion

        #region 2
        /// <summary>
        /// Avec utilisation d'un paramètre
        /// </summary>
        public static void Exec2()
        {
            // Utilisation d'un delegate, sans retour, qui prend un objet en entré
            Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod2));
            t.Start(5);
            t.Join();
        }

        public static void ThreadMethod2(object count)
        {
            for (int i = 0; i < (int)count; i++)
            {
                Trace.WriteLine(string.Format("ThreadProc:{0}", i));
                Console.WriteLine("ThreadProc:{0}", i);
                Thread.Sleep(0);
            }

        }
        #endregion

        #region 3 
        ///Stopping a thread

        public static void Exec3()
        {
            bool stopped = false;
            
            Thread t = new Thread(new ThreadStart(() =>
            {
                while (!stopped)
                {
                    Trace.WriteLine("Running...");
                    Thread.Sleep(1000);
                }
            }));

            t.Start();
            Console.WriteLine("Pressanykeytoexit");
            Console.ReadKey();
            stopped = true;
            t.Join();
        }
        
        #endregion

    }
}
