using System;

namespace SyntaxAnalysis
{
    public class Braces
    {
        public Braces() { }


        static void DoSth()
        {
            Console.WriteLine("element");
        }

        public void Action()
        {
            bool b = false;
            if (b)
            {
                if (b)
                {
                    DoSth();
                }
                else
                {

                }

                DoSth();
                DoSth();
                int x;
                int a = 1;
                int c;
                int g = b ? 1 : 0;
                x = a;
                c = x;
                a = c;
                g = a;
                Console.WriteLine(g);
            }
            if (b)
            {

            }
            else if (b)
            {
                if (b)
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }

    }
}
