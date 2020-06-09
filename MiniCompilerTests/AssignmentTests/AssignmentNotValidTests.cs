using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class AssignmentNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestUndeclaredVariable()
        {
            ExpectedErrors = 1;
            Invoke();
        }

        [TestMethod]
        public void TestAssignToValue()
        {
            ExpectedErrors = 7;
            Invoke();
        }


        [TestMethod]
        public void TestAssignWithDeclaration()
        {
            ExpectedErrors = 5;
            Invoke();
        }

        [TestMethod]
        public void TestUnmatchingTypes()
        {
            ExpectedErrors = 9;
            Invoke();
        }

        [TestMethod, Timeout(2 * DefaultTimeout)]
        public void TestInvalidValueFormatInt()
        {
            ExpectedErrors = 3;
            Invoke();
        }

        [TestMethod, Timeout(3 * DefaultTimeout)]
        public void TestInvalidValueFormatDoubleDots()
        {
            ExpectedErrors = 7;
            Invoke();
        }

        [TestMethod, Timeout(2 * DefaultTimeout)]
        public void TestInvalidValueFormatDoubleZeros()
        {
            ExpectedErrors = 3;
            Invoke();
        }

        [TestMethod]
        public void TestWithCasting()
        {
            Invoke();
        }
    }
}
