using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Math
{
    [TestClass]
    public class CombinedNotValidTests : MathNotValidTests
    {
        [TestMethod]
        public void TestInvalidTypeSimple()
        {
            Invoke();
        }

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
