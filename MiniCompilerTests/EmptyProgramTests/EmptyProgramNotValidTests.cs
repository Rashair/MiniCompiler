using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;

namespace MiniCompilerTests
{
    [TestClass]
    public class EmptyProgramNotValidTests : NotValidTests
    {
        [TestMethod]
        public void TestNoProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestProgramNoBraces()
        {
            Invoke();
        }

        [TestMethod]
        public void TestProgramNoOpeningBrace()
        {
            Invoke();
        }

        [TestMethod]
        public void TestProgramNoClosingBrace()
        {
            Invoke();
        }


        [TestMethod]
        public void TestProgramWrongBraceOrder()
        {
            Invoke();
        }

        [TestMethod]
        public void TestBracesBeforeProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestProgramCommentedBraces()
        {
            Invoke();
        }

        [TestMethod]
        public void TestCommentAtTheEndOfFile()
        {
            Invoke();
        }
    }
}
