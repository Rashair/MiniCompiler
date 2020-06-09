using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class LessOrEqualNotValidTests : RelationNotValidTests
    {
        [TestMethod]
        public void TestNoSemicolon()
        {
            Invoke();
        }
    }
}
