using System.IO;

namespace MiniCompilerTests.Operators.Logical
{
    public class LogicalNotValidTests : OperatorsNotValidTests 
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Logical");
    }
}
