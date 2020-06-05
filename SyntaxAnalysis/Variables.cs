using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalysis
{
    class Variables
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
