using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.IOStream
{
    [TestClass]
    public class WriteValidTests : IOStreamValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestManyCombinations()
        {
            Invoke();
        }

        [TestMethod]
        public void TestEscaping()
        {
            Invoke();
        }

        [TestMethod]
        public void TestDifferentNewLine()
        {
            Invoke();
        }

        [TestMethod]
        public void TestEscapingHard()
        {
            Invoke();
        }
    }
}