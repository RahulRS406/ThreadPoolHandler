using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Approach2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Func<string, string>> listOfFuncs = new List<Func<string, string>>();
            Func<string, string> func1 = (i) =>
            {
                Console.WriteLine($"Func1: ABC{i} on thread {Thread.CurrentThread.ManagedThreadId}");
                return $"ABC{i}";
            };
            Func<string, string> func2 = (i) =>
            {
                Console.WriteLine($"Func2: XYZ{i} on thread {Thread.CurrentThread.ManagedThreadId}");
              return  $"XYZ{i}";
            };
            listOfFuncs.Add(func1);
            listOfFuncs.Add(func2);

            var list =FuncHandler.ExecuteParallel(listOfFuncs, 2);

            Console.WriteLine("The following are the functions and its corresponding values: ");
            list.ToList().ForEach(func =>
            {
                Console.WriteLine(func.Key);
                Console.WriteLine(func.Value);
            });
            Console.WriteLine($"No of functions returned: {list.Count()}");
            Console.ReadLine();
        }
    }

    public static class FuncHandler
    {
        public static IEnumerable<KeyValuePair<Func<string, string>, object>> ExecuteParallel(IList<Func<string, string>> funcsToExecute, int maxNoOfThreads)
        {
            ParallelOptions parallelOpns = new ParallelOptions {MaxDegreeOfParallelism = maxNoOfThreads};
            List<KeyValuePair<Func<string, string>, object>> keyValuePairs = new List<KeyValuePair<Func<string, string>, object>>();

            Parallel.For(0,maxNoOfThreads, parallelOpns,(i) => { keyValuePairs.Add(new KeyValuePair<Func<string, string>, object> (funcsToExecute[i],ExecuteFunc(funcsToExecute[i],"AA")));});
            return keyValuePairs;
        }


        public static KeyValuePair<Func<string,string>, object> ExecuteFunc(Func<string, string> func, string character)
        {
            var result = func(character);
            return new KeyValuePair<System.Func<string, string>, object>(func, result);
        }
    }
}
