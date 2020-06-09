using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class LessOrEqualValidTests : RelationValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}