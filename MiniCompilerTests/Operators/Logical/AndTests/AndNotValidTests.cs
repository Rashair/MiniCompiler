using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Logical
{
    [TestClass]
    public class AndNotValidTests : LogicalNotValidTests
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