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
    public static class PartThread
    {
        public static void Run()
        {
            Exec5();
        }

        #region 1 - Thread Background, Foreground, Join

        /// <summary>
        /// Illustration d'un appel de méthode sur un autre thread.
        /// Ex : Background et Foreground thread
        /// Ex : Join
        /// </summary>
        public static void Exec1()
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod1));

            // Foreground vs background.
            // If declare as background, as soon as the Exec1 method is over,  the other thread such as t will close (even if unfinished).
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

        #region 2 - Thread - Utilisation d'un paramètre via une méthode delegate (ParameterizedThreadStart)
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

        #region 3 - Stopping a thread

        public static void ThreadMethod3(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                Console.WriteLine($"ThreadProc:{i}");
                Thread.Sleep(0);
            }
        }

        public static void Exec3()
        {
            bool stopped = false;

            Thread t = new Thread(new ThreadStart(() =>
            {

                while (!stopped)
                {
                    Console.WriteLine("Running...");
                    Thread.Sleep(1000);
                }
            }));

            t.Start();
            Console.WriteLine("Pressanykeytoexit");
            Console.ReadKey();
            stopped = true;
            t.Join(); // Empeche de fermer le thread principal tant que les autres threads ne sont pas terminés

        }











        #endregion

        #region 4 -  ThreadStaticAttribute
        // A thread has its own call stack that stores all the methods that are executed. Local variables
        // are stored on the call stack and are private to the thread.
        // A thread can also have its own data that’s not a local variable.By marking a field with the
        // ThreadStatic attribute, each thread gets its own copy of a field(see Listing 1-5).

        [ThreadStatic]
        public static int _fieldThreadStatic;
        public static int _field;

        public static void Exec4()
        {
            new Thread(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    _field++;
                    _fieldThreadStatic++;
                    Trace.WriteLine($"ThreadA: static : {_fieldThreadStatic}");
                    Trace.WriteLine($"ThreadA:{_field}");
                }
            }).Start();

            new Thread(() =>
             {
                 for (int x = 0; x < 10; x++)
                 {
                     _field++;
                     _fieldThreadStatic++;
                     Trace.WriteLine($"Thread B static: {_fieldThreadStatic}");
                     Trace.WriteLine($"Thread B: {_field}");
                 }
             }).Start();
        }


        #endregion

        #region 5 - ThreadLocal<T>

        // If you want to use local data in a thread and initialize it for each thread, you can use the
        // ThreadLocal<T> class. This class takes a delegate to a method that initializes the value.Listing
        // 1-6 shows an example

        public static ThreadLocal<int> _field5 = new ThreadLocal<int>(() =>
        {
            return Thread.CurrentThread.ManagedThreadId;
        });

        public static void Exec5()
        {
            Trace.WriteLine($"Thread Current Id : {Thread.CurrentThread.ManagedThreadId}");
            new Thread(() =>
            {
                Trace.WriteLine($"Thread A Id : {Thread.CurrentThread.ManagedThreadId}");
                for (int x = 0; x < _field5.Value; x++)
                {
                    Trace.WriteLine($"ThreadA:{x}");
                }
            }).Start();

            new Thread(() =>
            {
                Trace.WriteLine($"Thread B Id : {Thread.CurrentThread.ManagedThreadId}");
                for (int x = 0; x < _field5.Value; x++)
                {
                    Trace.WriteLine($"Threadb:{x}");
                }
            }).Start();

            Console.ReadKey();
        }
        #endregion
    }
}
