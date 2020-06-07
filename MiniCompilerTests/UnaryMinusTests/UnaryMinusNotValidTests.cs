using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;

namespace MiniCompilerTests
{
    [TestClass]
    public class UnaryMinusNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}
