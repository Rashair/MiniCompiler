using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class StatementsNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestWrongCase()
        {
            Invoke();
        }
    }
}
