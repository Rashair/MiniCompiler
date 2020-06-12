using MiniCompiler.Syntax.General;
using MiniCompiler.Syntax.IOStream;
using MiniCompiler.Syntax.Operators.Assignment;
using MiniCompiler.Syntax.Operators.Bitwise;
using MiniCompiler.Syntax.Operators.Logical;
using MiniCompiler.Syntax.Operators.Math;
using MiniCompiler.Syntax.Operators.Relation;
using MiniCompiler.Syntax.Operators.Unary;
using MiniCompiler.Syntax.Variables;
using MiniCompiler.Syntax.Variables.Scopes;
using System;

namespace MiniCompiler.Syntax
{
    public class SyntaxVisitor
    {
        private readonly SyntaxTree tree;

        public SyntaxVisitor(SyntaxTree tree)
        {
            this.tree = tree;
        }

        public void Visit()
        {
            tree.Visit(this);
        }

        public void Visit(CompilationUnit compilationUnit)
        {
            Compiler.EmitCode("ldstr \"\\nEnd of execution\\n\"");
            Compiler.EmitCode("call void [mscorlib]System.Console::WriteLine(string)");
            Compiler.EmitCode("");

            compilationUnit.Child.Visit(this);
        }

        public void Visit(UnaryMinus unaryMinus)
        {

        }

        public void Visit(GreaterOrEqual greaterOrEqual)
        {

        }

        public void Visit(LogicNegation logicNegation)
        {

        }

        public void Visit(Or or)
        {

        }

        public void Visit(Less less)
        {

        }

        public void Visit(Value value)
        {

        }

        public void Visit(VariableReference variableReference)
        {

        }

        public void Visit(NotEquals notEquals)
        {

        }

        public void Visit(VariableDeclaration variableDeclaration)
        {

        }

        public void Visit(LessOrEqual lessOrEqual)
        {

        }

        public void Visit(And and)
        {

        }

        public void Visit(Equals equals)
        {

        }

        public void Visit(Greater greater)
        {

        }

        public void Visit(Subtract subtract)
        {

        }

        public void Visit(Multiplies multiplies)
        {

        }

        public void Visit(Divides divides)
        {

        }

        public void Visit(BitOr bitOr)
        {

        }

        public void Visit(Add add)
        {

        }

        public void Visit(IfCond ifCond)
        {

        }

        public void Visit(BitAnd bitAnd)
        {

        }

        public void Visit(Assign assign)
        {

        }

        public void Visit(Read read)
        {

        }

        public void Visit(ElseCond elseCond)
        {

        }

        public void Visit(Block blocks)
        {

        }

        public void Visit(WhileLoop whileLoop)
        {

        }

        public void Visit(Write write)
        {

        }

        public void Visit(IntCast intCast)
        {

        }

        public void Visit(BitNegation bitNegation)
        {

        }

        public void Visit(DoubleCast doubleCast)
        {

        }

        public void Visit(Return @return)
        {

        }

        public void Visit(SimpleString simpleString)
        {

        }

        private void VisitAllChildren(SyntaxNode node)
        {
            for (int i = 0; i < node.Count; ++i)
            {
                node[i].Visit(this);
            }
        }
    }
}