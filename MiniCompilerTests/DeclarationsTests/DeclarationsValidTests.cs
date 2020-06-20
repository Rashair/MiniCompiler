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
                    new VariableDeclaration("a", mainScope, MiniType.Int),
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
                    new VariableDeclaration("a", mainScope, MiniType.Int),
                    new VariableDeclaration("b", mainScope, MiniType.Double),
                    new VariableDeclaration("c", mainScope, MiniType.Bool),
                    new VariableDeclaration("d", mainScope, MiniType.Bool),
                    new VariableDeclaration("e", mainScope, MiniType.Int),
                    new VariableDeclaration("g", mainScope, MiniType.Double),
                    new VariableDeclaration("aa", mainScope, MiniType.Double),
                    new VariableDeclaration("ab", mainScope, MiniType.Int),
                    new VariableDeclaration("cc", mainScope, MiniType.Int),
                    new VariableDeclaration("dd", mainScope, MiniType.Double),
                    new VariableDeclaration("eee", mainScope, MiniType.Bool),
                    new VariableDeclaration("gag", mainScope, MiniType.Double),
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
                    new VariableDeclaration("a", mainScope, MiniType.Double),
                    new VariableDeclaration("amuuuuuuuuuuuused", mainScope, MiniType.Double),
                    new VariableDeclaration("b", mainScope, MiniType.Bool),
                    new VariableDeclaration("c", mainScope, MiniType.Int),
                    new VariableDeclaration("abracadabra", mainScope, MiniType.Int),
                    new VariableDeclaration("gfgeeeaaaar", mainScope, MiniType.Bool),
                    new VariableDeclaration("d", mainScope, MiniType.Double),
                    new VariableDeclaration("e", mainScope, MiniType.Int),
                    new VariableDeclaration("g", mainScope, MiniType.Double),
                    new VariableDeclaration("kaladin", mainScope, MiniType.Int),
                    new VariableDeclaration("notreallyused", mainScope, MiniType.Bool),
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
                    new VariableDeclaration("aA", mainScope, MiniType.Int),
                    new VariableDeclaration("aa", mainScope, MiniType.Int),
                    new VariableDeclaration("Aa", mainScope, MiniType.Bool),
                    new VariableDeclaration("AA", mainScope, MiniType.Double),
                    new VariableDeclaration("bbBb", mainScope, MiniType.Int),
                    new VariableDeclaration("bbbB", mainScope, MiniType.Int),
                    new VariableDeclaration("ee", mainScope, MiniType.Double),
                    new VariableDeclaration("eE", mainScope, MiniType.Int),
                    new VariableDeclaration("gA", mainScope, MiniType.Double),
                    new VariableDeclaration("GA", mainScope, MiniType.Bool),
                    new VariableDeclaration("Kaladin", mainScope, MiniType.Int),
                    new VariableDeclaration("kaladin", mainScope, MiniType.Bool),
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
                        new VariableDeclaration("a", scopes[0], MiniType.Double),
                    },
                    new Block
                    {
                        new VariableDeclaration("amuuuuuuuuuuuused", scopes[1], MiniType.Double),
                        new VariableDeclaration("b", scopes[1], MiniType.Bool),
                        new VariableDeclaration("c", scopes[1], MiniType.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("abracadabra", scopes[2], MiniType.Int),
                        new Block
                        {
                            new VariableDeclaration("gfgeeeaaaar", scope20, MiniType.Bool),
                            new Block(),
                        },
                    },
                    new Block
                    {
                        new VariableDeclaration("d", scopes[3], MiniType.Double),
                        new VariableDeclaration("e", scopes[3], MiniType.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("g", scopes[4], MiniType.Double),
                    },
                    new Block
                    {
                    new VariableDeclaration("kaladin", scopes[5], MiniType.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("notreallyused", scopes[6], MiniType.Bool),
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
                    new VariableDeclaration("aa", mainScope, MiniType.Int),
                    new VariableDeclaration("ee", mainScope, MiniType.Int),
                    new VariableDeclaration("bbbb", mainScope, MiniType.Int),
                    new Block
                    {
                        new VariableDeclaration("aa", scopes[0], MiniType.Int),
                        new VariableDeclaration("bb", scopes[0], MiniType.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("bb", scopes[1], MiniType.Bool),
                        new VariableDeclaration("aa", scopes[1], MiniType.Double),
                    },
                    new Block
                    {
                        new VariableDeclaration("bbbb", scopes[2], MiniType.Int),
                    },
                    new Block
                    {
                        new VariableDeclaration("ee", scopes[3], MiniType.Double),
                        new Block
                        {
                            new VariableDeclaration("bbbb", scope30, MiniType.Int),
                        },
                    },
                    new Block
                    {
                        new VariableDeclaration("ee", scopes[4], MiniType.Bool),
                    },
                    new Block
                    {
                        new VariableDeclaration("aa", scopes[5], MiniType.Int),
                        new Block
                        {
                            new VariableDeclaration("aa", scope50, MiniType.Int),
                            new Block
                            {
                                new VariableDeclaration("aa", scope500, MiniType.Int),
                                new VariableDeclaration("bb", scope500, MiniType.Bool),
                                new VariableDeclaration("ee", scope500, MiniType.Double),
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

        [TestMethod]
        public void TestMultilineStatement()
        {
            Invoke();
        }

        [TestMethod]
        public void TestTwoScopes()
        {
            Invoke();
        }

        [TestMethod]
        public void TestNameSameAsTypeDifferentCase()
        {
            Invoke();
        }

        [TestMethod]
        public void TestNameSameAsKeywordDifferentCase()
        {
            Invoke();
        }

        [TestMethod]
        public void TestAssemblerKeywords()
        {
            Invoke();
        }
    }
}