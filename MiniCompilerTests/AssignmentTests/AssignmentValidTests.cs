using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;
using MiniCompiler.Extensions;
using MiniCompiler.Syntax.General;
using MiniCompiler.Syntax.Operators.Assignment;
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
            var declare = new VariableDeclaration("a", scope, Type.Int);
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block()
                {
                    declare,
                    Operator.Create(Token.Assign, Type.Int, Type.Int)
                    .WithLeft(new VariableReference(declare))
                    .WithRight(new Value(Type.Int, "5"))
                }
            );

            Invoke();
        }

        [TestMethod]
        public void TestMultipleAssign()
        {
            var scope = new SubordinateScope(new EmptyScope());
            var declare0 = new VariableDeclaration("a", scope, Type.Int);
            var declare1 = new VariableDeclaration("b", scope, Type.Int);
            var declare2 = new VariableDeclaration("c", scope, Type.Int);
            ExpectedTree = Helpers.CreateSyntaxTree(
                new Block()
                {
                    declare0,
                    declare1,
                    declare2,
                    Operator.Create(Token.Assign, Type.Int, Type.Int)
                    .WithLeft(new VariableReference(declare1))
                    .WithRight(
                        Operator.Create(Token.Assign, Type.Int, Type.Int)
                         .WithLeft(new VariableReference(declare0))
                         .WithRight(
                            Operator.Create(Token.Assign, Type.Int, Type.Int)
                            .WithLeft( new VariableReference(declare2))
                            .WithRight( new Value(Type.Int, "0"))

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
    }
}