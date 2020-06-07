using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler.Syntax;
using MiniCompiler.Syntax.General;

namespace MiniCompilerTests
{
    [TestClass]
    public class EmptyProgramValidTests : ValidTests
    {
        [TestInitialize]
        public override void Init()
        {
            base.Init();
            ExpectedTree = new SyntaxTree(
                new CompilationUnit()
                {
                    Child = new Block(),
                });
        }


        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestThreeLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestTwoLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceInsideBraces()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceBeforeProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceAfterProgramBeforeBraces()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWhitespaceAfterBraces()
        {
            Invoke();
        }

        [TestMethod]
        public void TestManySemicolons()
        {
            Invoke();
        }

        [TestMethod]
        public void TestComments()
        {
            Invoke();
        }
    }
}
