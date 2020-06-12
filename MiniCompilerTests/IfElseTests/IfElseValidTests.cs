using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class IfElseValidTests : ValidTests
    {
        [TestMethod]
        public void TestNested()
        {
            Invoke();
        }
    }
}