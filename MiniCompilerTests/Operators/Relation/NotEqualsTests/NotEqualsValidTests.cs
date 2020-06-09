using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class NotEqualsValidTests : RelationValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}