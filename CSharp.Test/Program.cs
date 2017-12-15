using Certification.ManageFlow;
using CSharp.Test.Certification.ManageFlow._05.Async;
using CSharp.Test.TableauApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace CSharp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.WriteLine("Début appel");
            //ExpressionLambda.Run();
            //PartThread.Run();
            //PoolThread.Run();
            //PartTask.Run();
            //PartParallelTask.Run();
            //DotNetBlog.Run();
            //AsyncPart.Run();

            ManagerRun.Run();

            Trace.WriteLine("Fin appel");
            //Console.ReadLine();
        }
        
        public static void AnonymousList()
        {
            var test = new List<int>() { 2 };
            
            var test2 = new List<dynamic>() { {new Tuple<int, int, string>(3, 4, "toto") } , { 2 } };

            test2.Select(s => new { Test = "Toto" });
            
            var test3 = new List<dynamic>() { new { Test = "toto".ToUpper() } };
            test3.First(s => s.Test);

        }
    }
}
