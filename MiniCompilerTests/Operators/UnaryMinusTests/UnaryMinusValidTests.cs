using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators
{
    [TestClass]
    public class UnaryMinusValidTests : OperatorsValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}
