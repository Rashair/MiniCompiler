using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;
using MiniCompiler.Syntax;

namespace MiniCompilerTests
{
    public class ValidTests : BaseTests
    {
        public ValidTests() :
            base(0, Helpers.Self())
        {
        }

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            ExpectedTree = null;
        }

        protected SyntaxTree ExpectedTree { get; set; }

        protected override void AssertCorrect()
        {
            if (ExpectedTree != null)
            {
                SyntaxTree syntaxTree = Compiler.parser.SyntaxTree;
                Assert.AreEqual(ExpectedTree, syntaxTree, "Trees should be equal");
            }
        }
    }
}
