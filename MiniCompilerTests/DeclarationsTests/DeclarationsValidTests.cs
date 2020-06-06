using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class DeclarationsValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestManyVariablesInOneLine()
        {
            Invoke();
        }

        [TestMethod]
        public void TestManyVariablesInManyLines()
        {
            Invoke();
        }

        [TestMethod]
        public void TestSameNamesDifferentCase()
        {
            Invoke();
        }
    }
}
