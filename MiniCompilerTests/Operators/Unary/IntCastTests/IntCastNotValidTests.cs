using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Unary
{
    [TestClass]
    public class IntCastNotValidTests : UnaryNotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}