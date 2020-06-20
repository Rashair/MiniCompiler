using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators
{
    [TestClass]
    public class PriorityValidTests : OperatorsValidTests
    {
        [TestMethod]
        public void TestInvalidTypeWhenPriorityWrong()
        {
            Invoke();
        }

        [TestMethod]
        public void TestUltimate()
        {
            Invoke();
        }
    }
}