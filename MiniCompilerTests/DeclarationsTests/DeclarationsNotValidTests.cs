using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class DeclarationsNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestWithoutSemicolon()
        {
            Invoke();
        }
    }
}
