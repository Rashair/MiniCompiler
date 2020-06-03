using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler.Syntax;
using MiniCompiler.Syntax.General;

namespace MiniCompilerTests
{
    [TestClass]
    public class BracesValidTests : ValidTests
    {
        [TestMethod]
        public void TestNestedEmptyBraces()
        {
            ExpectedTree = new SyntaxTree(
                new CompilationUnit()
                {
                    new Block()
                    {
                        new Block(),
                        new Block()
                        {
                            new Block(),
                            new Block(),
                            new Block(),
                        },
                        new Block(),
                        new Block(),
                        new Block()
                        {
                            new Block(),
                        },
                        new Block(),
                    }
                }
            );

            Invoke();
        }
    }
}
