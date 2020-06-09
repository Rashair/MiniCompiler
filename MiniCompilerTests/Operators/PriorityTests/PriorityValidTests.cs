using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MiniCompilerTests.Operators
{
    [TestClass]
    public class PriorityValidTests : OperatorsValidTests
    {
        [TestMethod]
        public void TestInvalidTypeWhenPriorityWrong()
        {
            Invoke();
        }
    }
}
