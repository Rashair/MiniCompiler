using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler.Syntax;
using MiniCompiler.Syntax.General;

namespace MiniCompilerTests
{
    [TestClass]
    public class BitNegationValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}
