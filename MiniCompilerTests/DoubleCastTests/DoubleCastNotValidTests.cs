using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;

namespace MiniCompilerTests
{
    [TestClass]
    public class DoubleCastNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }
    }
}
