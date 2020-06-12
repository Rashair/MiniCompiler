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

            Test();
            Console.WriteLine(a);

            bool b;
            b = true;

            if(b || false)
            {
                double d;
                d = 5.55;
                Console.Write(string.Format(CultureInfo.InvariantCulture, "{0:0.000000}", 5.5 + d / 2.0));
            }

            Console.WriteLine(b);
            Console.WriteLine(c);
        }
    }
}
