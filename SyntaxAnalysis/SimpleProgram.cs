using System;

namespace SyntaxAnalysis
{
    internal class SimpleProgram
    {
        private static void Test()
        {
            int a;
            a = 5;
            Console.Write(a);
        }

        private static void Main(string[] args)
        {
            int k = int.Parse(Console.ReadLine());
            double d = double.Parse(Console.ReadLine());
            bool b = bool.Parse(Console.ReadLine());
        }
    }
}