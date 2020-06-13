using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class FullProgramValidTests : ValidTests
    {
        [TestMethod]
        public void TestPrintAllPrimeNumbers()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMonteCarlo()
        {
            Invoke();
        }
    }
}