using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class EmptyProgramValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestThreeLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceInsideBraces()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceBeforeProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceAfterProgramBeforeBraces()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceAfterBraces()
        {
            Invoke();
        }
    }
}
