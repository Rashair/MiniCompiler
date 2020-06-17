using System.IO;

namespace MiniCompilerTests.Operators.Math
{
    public class MathNotValidTests : OperatorsNotValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Math");
    }
}