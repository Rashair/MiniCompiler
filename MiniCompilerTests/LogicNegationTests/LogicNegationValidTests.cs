using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler.Syntax;
using MiniCompiler.Syntax.General;

namespace MiniCompilerTests
{
    [TestClass]
    public class LogicNegationValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}
