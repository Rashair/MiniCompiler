using System;
using System.Globalization;

namespace SyntaxAnalysis
{
    class SimpleProgram
    {
        static void Test()
        {
            int a;
            a = 5;
            Console.Write(a);
        }


        static void Main(string[] args)
        {
            string val;
            val = Console.ReadLine();
            string[] values = val.Split();
            int k = int.Parse(values[0]);
            bool.Parse(values[1]);
        }
    }
}
