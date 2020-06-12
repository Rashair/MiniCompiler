using System;

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

            int a;

            double c;
            a = 5;
            c = 1.0;

            Test();
            Console.WriteLine(a);

            bool b;
            b = true;
            Console.WriteLine(b);
            Console.WriteLine(c);
        }
    }
}
