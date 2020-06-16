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
    }
}