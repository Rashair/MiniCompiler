using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.IOStream
{
    [TestClass]
    public class ReadValidTests : IOStreamValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}