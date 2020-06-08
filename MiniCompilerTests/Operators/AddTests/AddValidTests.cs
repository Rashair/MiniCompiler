using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators
{
    [TestClass]
    public class AddValidTests : OperatorsValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultiLineStatement()
        {
            Invoke();
        }
    }
}
