using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Certification.ManageFlow
{

    // Queuing a work item to a thread pool can be useful, but it has its shortcomings. There is no
    // built-in way to know when the operation has finished and what the return value is.

    // This is why the .NET Framework introduces the concept of a Task, which is an object that
    // represents some work that should be done.The Task can tell you if the work is completed and
    // if the operation returns a result, the Task gives you the result.

    // A task scheduler is responsible for starting the Task and managing it. By default, the Task
    // scheduler uses threads from the thread pool to execute the Task.

    public static class PartTask
    {
        public static void Run()
        {
            Exec8();
        }

        #region  1 - Starting a new task

        public static void Exec1()
        {
            // Task will use PoolThread
            Task t = Task.Run(() =>
            {
                for (int x = 0; x < 100; x++)
                {
                    Trace.WriteLine('*');
                }
            });

            t.Wait(); // The same as Join when manually doing a thread
        }

        #endregion

        #region 2 - Starting a new task with result value

        public static void Exec2()
        {
            // Task will use PoolThread
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            // As long as the Task has not finished, it is impossible to give the result.
            // If the Task is not finished, this call will block the current thread.
            Trace.WriteLine(t.Result); // Displays 42

            t.Wait(); // The same as Join when manually doing a thread
        }

        #endregion

        #region 3 - Adding a continuation

        public static void Exec3()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i) =>
            {
                return i.Result * 2;
            });

            Trace.WriteLine(t.Result); // Displays 84

        }

        #endregion

        #region 4 -  Scheduling different continuation tasks

        public static void Exec4()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });
            t.ContinueWith((i) =>
            {
                Trace.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);
            t.ContinueWith((i) =>
            {
                Trace.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);
            var completedTask = t.ContinueWith((i) =>
            {
                Trace.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            completedTask.Wait();

            Trace.WriteLine(t.Result); // Displays 84

        }

        #endregion
        
        #region 5 -  Attaching child tasks to a parent task

        public static void Exec5()
        {
            
            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];
                new Task(() => results[0] = 0,
                TaskCreationOptions.AttachedToParent).Start();

                new Task(() => results[1] = 1,
                TaskCreationOptions.AttachedToParent).Start();

                new Task(() => results[2] = 2,
                TaskCreationOptions.AttachedToParent).Start();

                return results;

            });

            var finalTask = parent.ContinueWith(
            parentTask =>
            {
                foreach (int i in parentTask.Result)
                    Trace.WriteLine(i);
            });

            finalTask.Wait();

            // The finalTask runs only after the parent Task is finished, 
            // and the parent Task finishes when all three children are finished.
            // You can use this to create quite complex Task hierarchies that will go through all the steps you specified.
        }

        #endregion
        
        #region 6 -  Using a TaskFactory - <Simpligy 5

        public static void Exec6()
        {
            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];
                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent,
                    TaskContinuationOptions.ExecuteSynchronously);

                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);
                
                return results;

            });

            var finalTask = parent.ContinueWith(
            parentTask => {
                foreach (int i in parentTask.Result)
                    Trace.WriteLine(i);
            });

            finalTask.Wait();
        }

        #endregion

        #region 7 -  Using Task.WaitAll

        public static void Exec7()
        {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() => {
                Thread.Sleep(1000);
                Trace.WriteLine("1");
                return 1;
            });

            tasks[1] = Task.Run(() => {
                Thread.Sleep(1000);
                Trace.WriteLine("2");
                return 2;
            });

            tasks[2] = Task.Run(() => {
                Thread.Sleep(1000);
                Trace.WriteLine("3");
                return 3;
            }
            );

            Task.WaitAll(tasks); // WhenALl
        }

        #endregion
        
        #region 8 -  Using Task.WhenAny

        public static void Exec8()
        {
            Task<int>[] tasks = new Task<int>[3];

            tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
            tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
            tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });

            while (tasks.Length > 0)
            {
                int i = Task.WaitAny(tasks);
                Task<int> completedTask = tasks[i];
                Trace.WriteLine(completedTask.Result);
                var temp = tasks.ToList();
                temp.RemoveAt(i);
                tasks = temp.ToArray();
            }

            // In this example, you process a completed Task as soon as it finishes.
            // By keeping track of which Tasks are finished, you don’t have to wait until all Tasks have completed.
        }

        #endregion
    }

}