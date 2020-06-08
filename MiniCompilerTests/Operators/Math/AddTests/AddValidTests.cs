using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Math
{
    [TestClass]
    public class AddValidTests :  MathValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultilineStatement()
        {
            Invoke();
        }
    }
}
