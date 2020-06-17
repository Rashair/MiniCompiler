using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Logical
{
    [TestClass]
    public class AndValidTests : LogicalValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultilineStatement()
        {
            Invoke();
        }
    }
}