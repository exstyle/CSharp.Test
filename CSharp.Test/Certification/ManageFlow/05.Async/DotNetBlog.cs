using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Test.Certification.ManageFlow._05.Async
{
    public static class DotNetBlog
    {
        public static void Run()
        {
            var menu = 3;
            Launch(menu);
            Console.ReadLine();
        }

        static async void Launch(int testId)
        {
            var start = DateTime.Now;
            Console.WriteLine("[{0}] DEBUT", start);
            string result = string.Empty;
            switch (testId)
            {
                case 1: result = await DoMyTasksV1("test1"); break;
                case 2: result = await DoMyTasksV2("test2"); break;
                case 3: result = await DoMyTasksV3("test2"); break;
            }
            
            var end = DateTime.Now;
            Console.WriteLine("[{0}] Sortie: {1}", end, result);
            Console.WriteLine("[{0}] TOUTES LES TACHES SONT TERMINEES - Temps global: {1}", end, end - start);
        }
        
        static async Task<string> DoMyTasksV1(string message)
        {
            Console.WriteLine("[{0}] Entrée dans la méthode DoMyTasksV1...", DateTime.Now);
            var resource = new DummyDelayResource();
            await resource.SendEmailAsync();
            var number = await resource.GetRandomNumberAsync();
            var upper = await resource.GetSpecialStringAsync(message);
            Console.WriteLine("[{0}] Sortie de la méthode DoMyTasksV1.", DateTime.Now);
            return string.Format("{0}-{1}", number, upper);
        }

        static async Task<string> DoMyTasksV2(string message)
        {
            Console.WriteLine("[{0}] Entrée dans la méthode DoMyTasksV2...", DateTime.Now);
            var resource = new DummyDelayResource();
            var emailTask = resource.SendEmailAsync();
            var number = await resource.GetRandomNumberAsync();
            var upper = await resource.GetSpecialStringAsync(message);
            await emailTask;
            Console.WriteLine("[{0}] Sortie de la méthode DoMyTasksV2.", DateTime.Now);
            return string.Format("{0}-{1}", number, upper);
        }

        static async Task<string> DoMyTasksV3(string message)
        {
            Console.WriteLine("[{0}] Entrée dans la méthode DoMyTasksV3...", DateTime.Now);
            var resource = new DummyDelayResource();
            var emailTask = resource.SendEmailAsync();
            var numberTask = resource.GetRandomNumberAsync();
            var upperTask = resource.GetSpecialStringAsync(message);

            var number = await numberTask;
            var upper = await upperTask;
            await emailTask;
            Console.WriteLine("[{0}] Sortie de la méthode DoMyTasksV3.", DateTime.Now);
            return string.Format("{0}-{1}", number, upper);
        }
    }

    public class DummyDelayResource
    {
        public Task SendEmailAsync()
        {
            Console.WriteLine("[{0}] SendMail (fake)", DateTime.Now);
            return Task.Delay(2000);
        }

        public async Task<int> GetRandomNumberAsync()
        {
            Console.WriteLine("[{0}] GetRandomNumber", DateTime.Now);
            await Task.Delay(2000);
            return (new Random()).Next();
        }

        public async Task<string> GetSpecialStringAsync(string message)
        {
            Console.WriteLine("[{0}] GetSpecialString", DateTime.Now);
            await Task.Delay(2000);
            return string.IsNullOrEmpty(message) ? "<RIEN>" : message.ToUpper();
        }
    }
}
