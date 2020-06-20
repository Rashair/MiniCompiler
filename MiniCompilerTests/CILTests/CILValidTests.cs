using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class CILValidTests : ValidTests
    {
        [TestMethod]
        public void TestConfusedLabels()
        {
            Invoke();
        }
    }
}