using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators
{
    [TestClass]
    public class IntCastValidTests : OperatorsValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}
