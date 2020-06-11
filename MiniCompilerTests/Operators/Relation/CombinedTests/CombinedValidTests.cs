using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class CombinedValidTests : RelationValidTests
    {
        [TestMethod]
        public void TestAssignToVariable()
        {
            Invoke();
        }
    }
}