using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class DeclarationsNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestWithoutSemicolon()
        {
            Invoke();
        }

        [TestMethod]
        public void TestRedeclaration()
        {
            Invoke();
        }

        [TestMethod]
        public void TestRedeclarationInNestedScope()
        {
            Invoke();
        }

        [TestMethod]
        public void TestStartNameWithNumber()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWithoutName()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWithoutType()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWrongCharsInName()
        {
            Invoke();
        }

        [TestMethod, Timeout(100)]
        public void TestWrongCharsInNameOnly()
        {
            Invoke();
        }

        [TestMethod]
        public void TestDifferentTypeCaseEnd()
        {
            Invoke();
        }

        [TestMethod]
        public void TestDifferentTypeCaseMiddle()
        {
            Invoke();
        }

        [TestMethod]
        public void TestDifferentTypeCaseStart()
        {
            Invoke();
        }

        [TestMethod]
        public void TestNameSameAsKeyword()
        {
            Invoke();
        }

        [TestMethod]
        public void TestNameSameAsType()
        {
            Invoke();
        }
    }
}
