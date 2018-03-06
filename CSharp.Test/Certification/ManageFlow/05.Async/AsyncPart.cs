using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Certification.ManageFlow
{
    public static class AsyncPart
    {
        public static void Run()
        {
            PrintPageLength();
        }

        #region 1 - async and await

        public static void Exec1()
        {
            string result = DownloadContent1().Result;
            Trace.WriteLine(result);
        }
        
        public static async Task<string> DownloadContent1()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.microsoft.com");
                return result;
            }
        }

        #endregion

        #region 2 -  Scalability versus responsiveness

        public static Task SleepAsyncA(int millisecondsTimeout)
        {
            return Task.Run(() => Thread.Sleep(millisecondsTimeout));
        }

        public static Task SleepAsyncB(int millisecondsTimeout)
        {
            TaskCompletionSource<bool> tcs = null;
            var t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
            tcs = new TaskCompletionSource<bool>(t);
            t.Change(millisecondsTimeout, -1);
            return tcs.Task;
        }

        #endregion
        
        #region 3 - skeet1 

        static async Task<int> GetPageLengthAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                Task<string> fetchTextTask = client.GetStringAsync(url);
                int length = (await fetchTextTask).Length;
                int length2 = await RecupereInt();
                return length;
            }
        }

        static int GetPageLength(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(url).Result.Length;
            }
        }

        public static Task<int> RecupereInt()
        {
            return Task.Run(() => 2);
        }


        // Async et Await ont tous les deux des fonctions. 
        // await permet d'attendre l'execution d'une tache. On peut ainsi await une méthode non async, pour peu que la méthode appelée ramène une tache.
        // Par contre, on ne peut pas utiliser le mot clé await dans une méthode non typé async. Pourquoi ?
        // -    Parce que l'utilisation de await permet de sortir de la méthode courante, et retourné dans la méthode appelante de base. Ainsi, la méthode utilisant
        // un await doit être type comme asynchrone : La méthode sera retournée même si elle n'est pas fini entièrement. Une telle méthode, typé async, doit renvoyer
        // null, Task ou Task<T>
        // -    On peut executer notre propre code pendant qu'une des méthodes est en await. Notre propre code ne doit pas faire du await.
        // Exemple : 

        /// <summary>
        /// Comme si cétait le thread principal. On va utiliser une méthode Aync : GetPageLengthAsync
        /// Cette méthode renvoi une tache<int>. Dans cette tâche, on utilise le mot clé await lors d'un appel à une fonction async de HttpClient
        /// Quand on tape sur le await, on sort de la méthode GetPageLengthAsync, on a créer une continuation après le await.
        /// Parce qu'on est sorti, on retourne à notre méthode ci-présence. Celle-ci n'a pas d'await. 
        /// La ligne TraceWriteLine("Before...") est donc exécuté. Ainsi que la seconde. Cette dernière, par contre, affiche le résultat final de la tache.
        /// Aussi, en pratique, elle ne s'affichera qu'après le await.
        /// </summary>
        static void PrintPageLength()
        {
            int lenghTaskSync = GetPageLength("http://csharpindepth.com");
            Task<int> lengthTask = GetPageLengthAsync("http://csharpindepth.com");
            Trace.WriteLine("Before the result thought declare a line after in code");
            Trace.WriteLine(lengthTask.Result);
        }

        #endregion

    }
}