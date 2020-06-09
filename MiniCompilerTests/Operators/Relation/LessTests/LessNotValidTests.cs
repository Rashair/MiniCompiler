using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class LessNotValidTests : RelationNotValidTests
    {
        [TestMethod]
        public void TestNoSemicolon()
        {
            Invoke();
        }
    }
}
