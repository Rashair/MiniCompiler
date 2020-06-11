using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class IfElseNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestNoParenth()
        {
            Invoke();
        }
    }
}
