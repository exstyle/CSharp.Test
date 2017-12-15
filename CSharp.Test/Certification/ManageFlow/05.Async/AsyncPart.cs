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