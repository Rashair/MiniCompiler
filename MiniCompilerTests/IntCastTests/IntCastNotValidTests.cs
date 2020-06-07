using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;

namespace MiniCompilerTests
{
    [TestClass]
    public class IntCastNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}
