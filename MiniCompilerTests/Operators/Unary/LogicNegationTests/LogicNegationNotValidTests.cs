using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Unary
{
    [TestClass]
    public class LogicNegationNotValidTests : UnaryNotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}