using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//#error version 
// c# 7.3

namespace funcs
{
    internal static class LongFunctions
    {
        public static int Add(int p1, int p2)
        {
            System.Threading.Thread.Sleep(1000);
            return p1 + p2;
        }

        public static string Stringify(int p)
        {
            System.Threading.Thread.Sleep(1000);
            return p.ToString();
        }

        public static int ParseInt(string p)
        {
            System.Threading.Thread.Sleep(1000);

            int.TryParse(p, out int ret);

            return ret;
        }
    }
}

namespace tools
{
    public static class MemoizeFunctionTools
    {
        public static Func<int, int, int> Memoizer(Func<int, int, int> func)
        {
            Dictionary<Tuple<int, int>, int> map = new Dictionary<Tuple<int, int>, int>();
            return (int p1, int p2) =>
            {
                var key = Tuple.Create(p1, p2);
                if (map.TryGetValue(key, out var value)) {
                    return value;
                }
                
                value = func(p1, p2);
                map[key] = value;
                return value;
            };
        }

        public static Func<Key, Value> TMemoizer<Key, Value>(Func<Key, Value> func)
        {
            Dictionary<Key, Value> map = new Dictionary<Key, Value>();
            return (Key param) =>
            {
                var key = param;
                if (map.TryGetValue(key, out var value))
                {
                    return value;
                }

                value = func(param);
                map[key] = value;
                return value;
            };
        }
    }

    public class Memoizer
    {
        readonly Func<int, int, int> m_func;
        readonly Dictionary<Tuple<int, int>, int> store = new Dictionary<Tuple<int, int>, int>() ;

        public Memoizer(Func<int, int, int> func)
        {
            m_func = func;
        }

        public int Compute(int p1, int p2)
        {
            var key = Tuple.Create(p1, p2);

            if (store.TryGetValue(key, out int ret))
            {
                return ret;
            }

            ret = m_func(p1, p2);
            store.Add(key, ret);
            return ret;
        }
        
    }

    public class TMemoizer<Key, Value>
    {
        readonly Func<Key, Value> m_func;
        readonly Dictionary<Key, Value> store = new Dictionary<Key, Value>();

        public TMemoizer(Func<Key, Value> func)
        {
            m_func = func;
        }

        public Value Compute(Key param)
        {
            var key = param;
            Value ret;
            if (store.TryGetValue(key, out ret))
            {
                return ret;
            }

            ret = m_func(param);
            store.Add(key, ret);
            return ret;
        }

    }


}

namespace Memoize
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                var memoizedAdd = new tools.Memoizer(funcs.LongFunctions.Add);

                Console.WriteLine("Started");

                Console.WriteLine($" memoized 1 + 2 = {memoizedAdd.Compute(1, 2)}");
                Console.WriteLine($" memoized 2 + 3 = {memoizedAdd.Compute(2, 3)}");
                Console.WriteLine($" memoized 3 + 4 = {memoizedAdd.Compute(3, 4)}");


                Console.WriteLine($" memoized 1 + 2 = {memoizedAdd.Compute(1, 2)}");
                Console.WriteLine($" memoized 2 + 3 = {memoizedAdd.Compute(2, 3)}");
                Console.WriteLine($" memoized 3 + 4 = {memoizedAdd.Compute(3, 4)}");

                Console.WriteLine("Ended");

            }
            {
                var memoizedParseInt = new tools.TMemoizer<string, int>(funcs.LongFunctions.ParseInt);
                Console.WriteLine("Started");

                Console.WriteLine($" memoized parseInt 1 = {memoizedParseInt.Compute("1")}");
                Console.WriteLine($" memoized parseInt 2 = {memoizedParseInt.Compute("2")}");
                Console.WriteLine($" memoized parseInt 3 = {memoizedParseInt.Compute("3")}");


                Console.WriteLine($" memoized parseInt 1 = {memoizedParseInt.Compute("1")}");
                Console.WriteLine($" memoized parseInt 2 = {memoizedParseInt.Compute("2")}");
                Console.WriteLine($" memoized parseInt 3 = {memoizedParseInt.Compute("3")}");

                Console.WriteLine("Ended");
            }

            {
                var memoizedAdd = tools.MemoizeFunctionTools.Memoizer(funcs.LongFunctions.Add);

                Console.WriteLine("Started");

                Console.WriteLine($" memoized 1 + 2 = {memoizedAdd(1, 2)}");
                Console.WriteLine($" memoized 2 + 3 = {memoizedAdd(2, 3)}");
                Console.WriteLine($" memoized 3 + 4 = {memoizedAdd(3, 4)}");


                Console.WriteLine($" memoized 1 + 2 = {memoizedAdd(1, 2)}");
                Console.WriteLine($" memoized 2 + 3 = {memoizedAdd(2, 3)}");
                Console.WriteLine($" memoized 3 + 4 = {memoizedAdd(3, 4)}");

                Console.WriteLine("Ended");

            }

            {
                var memoizedParseInt = tools.MemoizeFunctionTools.TMemoizer<string, int>(funcs.LongFunctions.ParseInt);
                Console.WriteLine("Started");

                Console.WriteLine($" memoized parseInt 1 = {memoizedParseInt("1")}");
                Console.WriteLine($" memoized parseInt 2 = {memoizedParseInt("2")}");
                Console.WriteLine($" memoized parseInt 3 = {memoizedParseInt("3")}");


                Console.WriteLine($" memoized parseInt 1 = {memoizedParseInt("1")}");
                Console.WriteLine($" memoized parseInt 2 = {memoizedParseInt("2")}");
                Console.WriteLine($" memoized parseInt 3 = {memoizedParseInt("3")}");

                Console.WriteLine("Ended");
            }

            System.Threading.Thread.Sleep(10000);

        }
    }
}
