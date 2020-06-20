using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;
using MiniCompiler.Extensions;
using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.General;
using MiniCompiler.Syntax.Variables;
using MiniCompiler.Syntax.Variables.Scopes;

namespace MiniCompilerTests
{
    [TestClass]
    public class AssignmentValidTests : ValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            var scope = new SubordinateScope(new EmptyScope());
            var declare = new VariableDeclaration("a", scope, MiniType.Int);
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block()
                {
                    declare,
                    Operator.Create(Token.Assign, MiniType.Int, MiniType.Int)
                    .WithLeft<Operator, TypeNode>(new VariableReference(declare))
                    .WithRight<Operator, TypeNode>(new Value(MiniType.Int, "5"))
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestMultipleAssign()
        {
            var scope = new SubordinateScope(new EmptyScope());
            var declare0 = new VariableDeclaration("a", scope, MiniType.Int);
            var declare1 = new VariableDeclaration("b", scope, MiniType.Int);
            var declare2 = new VariableDeclaration("c", scope, MiniType.Int);
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block()
                {
                    declare0,
                    declare1,
                    declare2,
                    Operator.Create(Token.Assign, MiniType.Int, MiniType.Int)
                    .WithLeft<Operator, TypeNode>(new VariableReference(declare1))
                    .WithRight<Operator, TypeNode>(
                        Operator.Create(Token.Assign, MiniType.Int, MiniType.Int)
                         .WithLeft<Operator, TypeNode>(new VariableReference(declare0))
                         .WithRight<Operator, TypeNode>(
                            Operator.Create(Token.Assign, MiniType.Int, MiniType.Int)
                            .WithLeft<Operator, TypeNode>( new VariableReference(declare2))
                            .WithRight<Operator, TypeNode>( new Value(MiniType.Int, "0"))

                         )
                    )
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestMultipleTypes()
        {
            Invoke();
        }

        [TestMethod]
        public void TestIntToDouble()
        {
            Invoke();
        }

        [TestMethod]
        public void TestVariableToVariable()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultilineStatement()
        {
            Invoke();
        }

        [TestMethod]
        public void TestWithCasting()
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