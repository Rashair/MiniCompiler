using System.IO;

namespace MiniCompilerTests.Operators.Relation
{
    public class RelationValidTests : OperatorsValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Relation");
    }
}