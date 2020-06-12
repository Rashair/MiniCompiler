using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Logical
{
    [TestClass]
    public class CombinedValidTests : LogicalValidTests
    {
        [TestMethod]
        public void TestManyCombinations()
        {
            Invoke();
        }
    }
}