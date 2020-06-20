using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace MiniCompilerTests.IOStream
{
    [TestClass]
    public class ReadValidTests : IOStreamValidTests
    {
        [TestMethod, Timeout(DefaultTimeout)]
        public void TestAllTypes()
        {
            Invoke();
        }

        [TestMethod]
        public void TestReadInWhile()
        {
            Invoke();
        }

        [TestMethod]
        public void TestReadInWhile1()
        {
            Invoke();
        }

        [TestMethod]
        public void TestReadInWhile2()
        {
            Invoke();
        }
    }
}