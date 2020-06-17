using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Unary
{
    [TestClass]
    public class UnaryMinusValidTests : UnaryValidTests
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

        [TestMethod]
        public void TestUsedManyTimesInRow()
        {
            Invoke();
        }
    }
}