using System.IO;

namespace MiniCompilerTests.Operators.Unary
{
    public class UnaryNotValidTests : OperatorsNotValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Unary");
    }
}