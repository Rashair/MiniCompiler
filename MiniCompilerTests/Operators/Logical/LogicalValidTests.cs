using System.IO;

namespace MiniCompilerTests.Operators.Logical
{
    public class LogicalValidTests : OperatorsValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Logical");
    }
}
