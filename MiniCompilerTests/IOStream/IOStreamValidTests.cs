using System.IO;

namespace MiniCompilerTests.IOStream
{
    public class IOStreamValidTests : ValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "IOStream");
    }
}
