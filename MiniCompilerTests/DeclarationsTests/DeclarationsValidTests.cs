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
            var mainScope = new SubordinateScope(new EmptyScope());
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block
                {
                    new VariableDeclaration("a", mainScope, Type.Double),
                    new VariableDeclaration("amuuuuuuuuuuuused", mainScope, Type.Double),
                    new VariableDeclaration("b", mainScope, Type.Bool),
                    new VariableDeclaration("c", mainScope, Type.Int),
                    new VariableDeclaration("abracadabra", mainScope, Type.Int),
                    new VariableDeclaration("gfgeeeaaaar", mainScope, Type.Bool),
                    new VariableDeclaration("d", mainScope, Type.Double),
                    new VariableDeclaration("e", mainScope, Type.Int),
                    new VariableDeclaration("g", mainScope, Type.Double),
                    new VariableDeclaration("kaladin", mainScope, Type.Int),
                    new VariableDeclaration("notreallyused", mainScope, Type.Bool),
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestSameNamesDifferentCase()
        {
            var mainScope = new SubordinateScope(new EmptyScope());
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block
                {
                    new VariableDeclaration("aA", mainScope, Type.Int),
                    new VariableDeclaration("aa", mainScope, Type.Int),
                    new VariableDeclaration("Aa", mainScope, Type.Bool),
                    new VariableDeclaration("AA", mainScope, Type.Double),
                    new VariableDeclaration("bbBb", mainScope, Type.Int),
                    new VariableDeclaration("bbbB", mainScope, Type.Int),
                    new VariableDeclaration("ee", mainScope, Type.Double),
                    new VariableDeclaration("eE", mainScope, Type.Int),
                    new VariableDeclaration("gA", mainScope, Type.Double),
                    new VariableDeclaration("GA", mainScope, Type.Bool),
                    new VariableDeclaration("Kaladin", mainScope, Type.Int),
                    new VariableDeclaration("kaladin", mainScope, Type.Bool),
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestManyVariablesInManyScopes()
        {
            var mainScope = new SubordinateScope(new EmptyScope());
            var scopes = Helpers.GenerateScopes(mainScope, 7);
            var scope20 = new SubordinateScope(scopes[2]);

            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block
                {
                    new Block
                    {
                        new VariableDeclaration("a", scopes[0], Type.Double),
                    },
                    new Block
                    {
                        new VariableDeclaration("amuuuuuuuuuuuused", scopes[1], Type.Double),
                        new VariableDeclaration("b", scopes[1], Type.Bool),
                        new VariableDeclaration("c", scopes[1], Type.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("abracadabra", scopes[2], Type.Int),
                        new Block
                        {
                            new VariableDeclaration("gfgeeeaaaar", scope20, Type.Bool),
                            new Block(),
                        },
                    },
                    new Block
                    {
                        new VariableDeclaration("d", scopes[3], Type.Double),
                        new VariableDeclaration("e", scopes[3], Type.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("g", scopes[4], Type.Double),
                    },
                    new Block
                    {
                    new VariableDeclaration("kaladin", scopes[5], Type.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("notreallyused", scopes[6], Type.Bool),
                    },
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestSameNamesDifferentScope()
        {
            var mainScope = new SubordinateScope(new EmptyScope());
            var scopes = Helpers.GenerateScopes(mainScope, 6);
            var scope30 = new SubordinateScope(scopes[3]);
            var scope50 = new SubordinateScope(scopes[5]);
            var scope500 = new SubordinateScope(scope50);


            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block
                {
                    new VariableDeclaration("aa", mainScope, Type.Int),
                    new Block
                    {
                        new VariableDeclaration("aa", scopes[0], Type.Int),
                        new VariableDeclaration("bb", scopes[0], Type.Int),
                    },
                    new Block 
                    {
                        new VariableDeclaration("bb", scopes[1], Type.Bool),
                        new VariableDeclaration("aa", scopes[1], Type.Double),
                    },
                    new VariableDeclaration("bbbb", mainScope, Type.Int),
                    new Block
                    {
                        new VariableDeclaration("bbbb", scopes[2], Type.Int),
                    },
                    new VariableDeclaration("ee", mainScope, Type.Int),
                    new Block
                    {
                        new VariableDeclaration("ee", scopes[3], Type.Double),
                        new Block
                        {
                            new VariableDeclaration("bbbb", scope30, Type.Int),
                        },
                    },
                    new Block
                    {
                        new VariableDeclaration("ee", scopes[4], Type.Bool),
                    },
                    new Block
                    {
                        new VariableDeclaration("aa", scopes[5], Type.Int),
                        new Block
                        {
                            new VariableDeclaration("aa", scope50, Type.Int),
                            new Block
                            {
                                new VariableDeclaration("aa", scope500, Type.Int),
                                new VariableDeclaration("bb", scope500, Type.Bool),
                                new VariableDeclaration("ee", scope500, Type.Double),
                            }
                        },
                    },
                }
            );

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
