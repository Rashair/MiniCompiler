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
