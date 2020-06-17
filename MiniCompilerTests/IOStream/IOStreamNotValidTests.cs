using System.IO;

namespace MiniCompilerTests.IOStream
{
    public class IOStreamNotValidTests : NotValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "IOStream");
    }
}