using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class EqualsNotValidTests : RelationNotValidTests
    {
        [TestMethod]
        public void TestNoSemicolon()
        {
            Invoke();
        }
    }
}
