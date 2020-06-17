using System.IO;

namespace MiniCompilerTests.Operators.Unary
{
    public class UnaryValidTests : OperatorsValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Unary");
    }
}