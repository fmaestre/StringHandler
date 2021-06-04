using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace ConsoleNetCore
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    // TaskContinueWith();
        //    // CancellationToken();
        //    // asyncMethodAsyn();
        //    //asynccallMethod();
        //    readFileAsync();
        //}

        static async Task Main(string[] args)
        {
            //await asynccallMethod();
            await RunWhenAll();
        }

        #region Task|ContinueWith
        static void TaskContinueWith()
        {
            Task.Run(() => Method1()).ContinueWith(task => Method2()).Wait();
        }
        static void Method1() { Console.WriteLine("::Method1::"); }
        static void Method2() { Console.WriteLine("::Method2::");
        }

        #endregion
        #region CancellationToken
        static void CancellationToken()
        {
            // Create CancellationTokenSource.
            var source = new CancellationTokenSource();
            // ... Get Token from source.
            var token = source.Token;

            // Run the DoSomething method and pass it the CancellationToken.
            // ... Specify the CancellationToken in Task.Run.
            var task = Task.Run(() => DoSomething(token), token);

            // Wait a few moments.
            Thread.Sleep(500);

            // Cancel the task.
            // ... This affects the CancellationTokens in the source.
            Console.WriteLine("Main::Canceling");
            source.Cancel();

            // Wait more.
            Thread.Sleep(500);
        }

        static void DoSomething(CancellationToken token)
        {
            // Do something important.
            for (int i = 0; i < 100; i++)
            {
                // Wait a few moments.
                Thread.Sleep(100);
                // See if we are canceled from our CancellationTokenSource.
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Method1 canceled");
                    return;
                }
                Console.WriteLine($"Method1 running... {i}");
            }
        }

        #endregion
        #region async await 0

        static void asyncMethodAsyn()
        {
            MethodAsync1();
            MethodAsync2();
        }


        public static async Task MethodAsync1()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(" Method 1");
                    // Do something
                    Task.Delay(100).Wait();
                }
            });
        }


        public static void MethodAsync2()
        {
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine(" Method 2");
                // Do something
                Task.Delay(100).Wait();
            }
        }
        #endregion
        #region async await 1

        public static async Task asynccallMethod()
        {
            MethodA();
            var count = await MethodB();
            MethodC(count);
        }

        public static async Task<int> MethodB()
        {
            int count = 0;
            await Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine(" Method 1");
                    count += 1;
                }
            });
            return count;
        }

        public static void MethodA()
        {
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine(" Method 2");
            }
        }

        public static void MethodC(int count)
        {
            Console.WriteLine("Total count is " + count);
        }

        #endregion
        #region readfile async

        static void readFileAsync()
        {
            Task task = new Task(CallMethod);
            task.Start();
            task.Wait();
            Console.ReadLine();
        }

        static async void CallMethod()
        {
            string filePath = @"\\Cavan01tsw004d\ax\Dev\Mods\Pendingmods.txt";
            Task<int> task = ReadFile(filePath);

            Console.WriteLine(" Other Work 1");
            Console.WriteLine(" Other Work 2");
            Console.WriteLine(" Other Work 3");

            try
            {
                int length = await task;
                Console.WriteLine(" Total length: " + length);

                Console.WriteLine(" After work 1");
                Console.WriteLine(" After work 2");
            }
            catch (Exception x)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error:" + x.Message  );
            }
        }

        static async Task<int> ReadFile(string file)
        {
            int length = 0;

            Console.WriteLine(" File reading is stating");
            using (StreamReader reader = new StreamReader(file))
            {
                // Reads all characters from the current position to the end of the stream asynchronously    
                // and returns them as one string.    
                string s = await reader.ReadToEndAsync();

                length = s.Length;
            }
            Console.WriteLine(" File reading is completed");
            return length;
        }

        #endregion

        #region WhenAll


        private static async Task RunWhenAll()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // this task will take about 2.5s to complete
            var sumTask = SlowAndComplexSumAsync();

            // this task will take about 4s to complete
            var wordTask = SlowAndComplexWordAsync();

            // running them in parallel should take about 4s to complete
            await Task.WhenAll(sumTask, wordTask);

            // The elapsed time at this point will only be about 4s
            Console.WriteLine("Time elapsed when both complete..." + stopwatch.Elapsed);

            // These lines are to prove the outputs are as expected,
            // i.e. 300 for the complex sum and "ABC...XYZ" for the complex word
            Console.WriteLine("Result of complex sum = " + sumTask.Result);
            Console.WriteLine("Result of complex letter processing " + wordTask.Result);

            Console.Read();
        }


        private static async Task<int> SlowAndComplexSumAsync()
        {
            int sum = 0;
            foreach (var counter in Enumerable.Range(0, 25))
            {
                sum += counter;
                await Task.Delay(100);
            }

            return sum;
        }


        private static async Task<string> SlowAndComplexWordAsync()
        {
            var word = string.Empty;
            foreach (var counter in Enumerable.Range(65, 26))
            {
                word = string.Concat(word, (char)counter);
                await Task.Delay(150);
            }

            return word;
        }


        #endregion

    }
}
