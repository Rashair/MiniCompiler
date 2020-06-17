using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Unary
{
    [TestClass]
    public class UnaryMinusNotValidTests : UnaryNotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}