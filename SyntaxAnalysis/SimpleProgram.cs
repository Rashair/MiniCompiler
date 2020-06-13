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

            int a = 5;
            int b = 6;
            int stloc;

            stloc = 5;

            if(a <= stloc)
            {
                Console.WriteLine("xx");
            }
        }
    }
}
