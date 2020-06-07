using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class AssignmentNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestUndeclaredVariable()
        {
            Invoke();
        }

        [TestMethod]
        public void TestAssignmentToNumber()

        {
            Invoke();
        }
    }
}
