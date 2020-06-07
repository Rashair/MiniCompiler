using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;

namespace MiniCompilerTests
{
    [TestClass]
    public class BitNegationNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}
