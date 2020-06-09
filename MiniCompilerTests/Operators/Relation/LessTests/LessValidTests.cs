using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class LessValidTests : RelationValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}