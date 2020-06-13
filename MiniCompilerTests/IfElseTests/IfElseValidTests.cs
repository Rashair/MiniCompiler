using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class IfElseValidTests : ValidTests
    {
        [TestMethod]
        public void TestNested()
        {
            Invoke();
        }

        [TestMethod]
        public void TestManyBinaryConditions()
        {
            Invoke();
        }

        [TestMethod]
        public void TestUnbalancedStack()
        {
            Invoke();
        }

        [TestMethod]
        public void TestAppropriateLabels()
        {
            Invoke();
        }
    }
}