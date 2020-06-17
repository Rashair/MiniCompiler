using System.IO;

namespace MiniCompilerTests.Operators.Relation
{
    public class RelationNotValidTests : OperatorsNotValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Relation");
    }
}