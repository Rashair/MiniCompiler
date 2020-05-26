using System.IO;
using System.Runtime.CompilerServices;

namespace MiniCompilerTests
{
    public static class Helpers
    {
        public static string Self([CallerFilePath] string callerFilePath = null)
        {
            var callerTypeName = Path.GetFileNameWithoutExtension(callerFilePath);
            System.Console.WriteLine(callerTypeName);

            return callerTypeName;
        }
    }
}
