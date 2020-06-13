using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.IOStream
{
    [TestClass]
    public class WriteNotValidTests : IOStreamNotValidTests
    {
        [TestMethod]
        public void TestWriteUndeclaredVariable()
        {
            Invoke();
        }

        [TestMethod]
        public void TestString()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWithEscapedQuotation()
        {
            Invoke();
        }
    }
}
