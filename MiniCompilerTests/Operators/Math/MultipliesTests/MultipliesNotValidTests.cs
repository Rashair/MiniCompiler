using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Math
{
    [TestClass]
    public class MultipliesNotValidTests : MathNotValidTests
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