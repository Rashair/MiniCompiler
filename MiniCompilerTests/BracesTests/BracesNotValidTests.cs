using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class BracesNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestNestedBracesMissingOpenAndClose()
        {
            Invoke();
        }

        [TestMethod]
        public void TestNestedBracesMissingOpen()
        {
            Invoke();
        }

        [TestMethod]
        public void TestNestedBracesMissingClose()
        {
            Invoke();
        }
    }
}
