using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class IntCastValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }
    }
}
