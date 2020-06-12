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

            int a;

            double c;
            a = 5;
            c = 1.0;

            //  Test();
            Console.WriteLine(a);
            a = a / 2;

            if(a == 5)
            {
                double d;
                d = 5.55;
                Console.Write(string.Format(CultureInfo.InvariantCulture, "{0:0.000000}", 5.5 + d / 2.0));
            }

            Console.WriteLine(true);
            Console.WriteLine(c);
        }
    }
}
