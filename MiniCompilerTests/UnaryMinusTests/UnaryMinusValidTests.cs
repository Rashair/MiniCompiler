using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class UnaryMinusValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}
