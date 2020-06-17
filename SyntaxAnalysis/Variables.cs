using System;

namespace SyntaxAnalysis
{
    internal class Variables
    {
        public void Action()
        {
            bool b = false;
            int a = 5;
            if (b)
            {
                double newA = a;
                double squared = newA * newA;
                a = (int)squared;
            }

            Console.WriteLine(a);
        }
    }
}