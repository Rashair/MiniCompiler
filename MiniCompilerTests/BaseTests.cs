using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompilerTests
{
    public abstract class BaseTests
    {
        public static string[] GetArgs(string arg)
        {
            return new string[] { arg };
        }
    }
}
