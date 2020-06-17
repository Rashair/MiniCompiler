using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.IOStream
{
    [TestClass]
    public class ReadNotValidTests : IOStreamNotValidTests
    {
        [TestMethod]
        public void TestReadUndeclaredVariable()
        {
            Invoke();
        }
    }
}