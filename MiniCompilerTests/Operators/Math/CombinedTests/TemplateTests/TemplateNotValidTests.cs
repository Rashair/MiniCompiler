using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class TemplateNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestNoSemicolon()
        {
            Invoke();
        }
    }
}
