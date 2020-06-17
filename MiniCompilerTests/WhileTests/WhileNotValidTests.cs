using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class WhileNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestInvalidFormat()
        {
            Invoke();
        }
    }
}