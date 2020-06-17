using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Math
{
    [TestClass]
    public class AddNotValidTests : MathNotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultipleUsage()
        {
            Invoke();
        }
    }
}