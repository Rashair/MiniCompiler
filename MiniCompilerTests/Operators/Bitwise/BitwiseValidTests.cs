using System.IO;

namespace MiniCompilerTests.Operators.Bitwise
{
    public class BitwiseValidTests : OperatorsValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Bitwise");
    }
}