using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Logical
{
    [TestClass]
    public class CombinedNotValidTests : LogicalNotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}