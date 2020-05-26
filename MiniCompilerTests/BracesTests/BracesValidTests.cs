using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class BracesValidTests : ValidTests
    {
        [TestMethod]
        public void TestNestedEmptyBraces()
        {
            Invoke();
        }
    }
}
