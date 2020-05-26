using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompilerTests
{
    public abstract class BaseTests
    {
        protected string testsPath;

        public BaseTests(string testsPath)
        {
            this.testsPath = Path.GetFullPath(Path.Combine(@"..\..\", testsPath));
        }

        public string GetPath(string name)
        {
            return Path.Combine(testsPath, $"{name}.txt");
        }

        public static string GetCaller([CallerMemberName] string caller = null)
        {
            return caller;
        }

        public static string[] GetArgs(string arg)
        {
            return new string[] { arg };
        }

        public static string GetCaseName(string methodName)
        {
            // length(Test)
            return methodName.Substring(4);
        }
    }
}
