using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class WhileValidTests : ValidTests
    {
        [TestMethod]
        public void TestManyDifferentConditions()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWithoutAddition()
        {
            Invoke();
        }

        [TestMethod]
        public void TestDeclareInLoop()
        {
            Invoke();
        }

        [TestMethod]
        public void TestReturnInLoop()
        {
            Invoke();
        }
    }
}