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
    }
}