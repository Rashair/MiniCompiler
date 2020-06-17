using System.IO;

namespace MiniCompilerTests.Operators.Bitwise
{
    public class BitwiseNotValidTests : OperatorsNotValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Bitwise");
    }
}