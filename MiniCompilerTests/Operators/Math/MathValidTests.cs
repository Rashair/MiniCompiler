using System.IO;

namespace MiniCompilerTests.Operators.Math
{
    public class MathValidTests : OperatorsValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Math");
    }
}
