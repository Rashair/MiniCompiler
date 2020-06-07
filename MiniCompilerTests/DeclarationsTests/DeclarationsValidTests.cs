using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler.Syntax.General;
using MiniCompiler.Syntax.Variables;
using MiniCompiler.Syntax.Variables.Scopes;

namespace MiniCompilerTests
{
    [TestClass]
    public class DeclarationsValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            var mainScope = new SubordinateScope(new EmptyScope());
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block
                {
                    new VariableDeclaration("a", mainScope, Type.Int),
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestManyVariablesInOneLine()
        {
            var mainScope = new SubordinateScope(new EmptyScope());
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block
                {
                    new VariableDeclaration("a", mainScope, Type.Int),
                    new VariableDeclaration("b", mainScope, Type.Double),
                    new VariableDeclaration("c", mainScope, Type.Bool),
                    new VariableDeclaration("d", mainScope, Type.Bool),
                    new VariableDeclaration("e", mainScope, Type.Int),
                    new VariableDeclaration("g", mainScope, Type.Double),
                    new VariableDeclaration("aa", mainScope, Type.Double),
                    new VariableDeclaration("ab", mainScope, Type.Int),
                    new VariableDeclaration("cc", mainScope, Type.Int),
                    new VariableDeclaration("dd", mainScope, Type.Double),
                    new VariableDeclaration("eee", mainScope, Type.Bool),
                    new VariableDeclaration("gag", mainScope, Type.Double),
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestManyVariablesInManyLines()
        {
            Invoke();
        }

        [TestMethod]
        public void TestSameNamesDifferentCase()
        {
            Invoke();
        }

        [TestMethod]
        public void TestManyVariablesInManyScopes()
        {
            Invoke();
        }

        [TestMethod]
        public void TestSameNamesDifferentScope()
        {
            Invoke();
        }

        [TestMethod]
        public void TestDifferentNames()
        {
            Invoke();
        }

        [TestMethod]
        public void TestCommentAfterDeclaration()
        {
            Invoke();
        }
    }
}
