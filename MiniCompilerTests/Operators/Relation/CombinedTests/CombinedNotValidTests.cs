using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class CombinedNotValidTests : RelationNotValidTests
    {
        [TestMethod]
        public void TestAssignToVariable()
        {
            Invoke();
        }
    }
}