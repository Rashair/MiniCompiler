using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class GreaterOrEqualValidTests : RelationValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}