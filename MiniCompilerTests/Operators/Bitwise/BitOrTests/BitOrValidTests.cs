using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Bitwise
{
    [TestClass]
    public class BitOrValidTests : BitwiseValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}