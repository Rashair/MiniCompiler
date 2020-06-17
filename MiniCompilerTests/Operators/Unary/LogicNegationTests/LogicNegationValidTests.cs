using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Unary
{
    [TestClass]
    public class LogicNegationValidTests : UnaryValidTests
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