using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators
{
    [TestClass]
    public class PriorityNotValidTests : OperatorsNotValidTests
    {
        [TestMethod]
        public void TestBitAndLogic()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMissingParentheses()
        {
            Invoke();
        }
    }
}