using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class StatementsValidTests : ValidTests
    {
        [TestMethod]
        public void TestSimpleStatements()
        {
            Invoke();
        }
    }
}