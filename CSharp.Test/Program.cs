using Certification.ManageFlow;
using System.Diagnostics;

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
            PartParallelTask.Run();
            
            Trace.WriteLine("Fin appel");
            //Console.ReadLine();
        }
    }
}
