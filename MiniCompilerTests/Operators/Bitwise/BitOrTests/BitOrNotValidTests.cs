using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Bitwise
{
    [TestClass]
    public class BitOrNotValidTests : BitwiseNotValidTests
    {
        [TestMethod]
        public void TestNoSemicolon()
        {
            Invoke();
        }
    }
}
