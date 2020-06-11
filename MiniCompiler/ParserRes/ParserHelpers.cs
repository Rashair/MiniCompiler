using MiniCompiler.Extensions;
using MiniCompiler.Syntax;
using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.General;
using MiniCompiler.Syntax.Variables;
using MiniCompiler.Syntax.Variables.Scopes;
using QUT.Gppg;
using System;

namespace MiniCompiler
{
    public partial class Parser
    {
        private IScope currentScope;
        private Token lastErrorToken;

        public Parser(Scanner scanner) : base(scanner)
        {
            currentScope = new EmptyScope();
        }

        public SyntaxTree SyntaxTree { get; private set; }

        public LexLocation Loc => CurrentLocationSpan;

        private void GenerateCode(CompilationUnit unit)
        {
            SyntaxTree = new SyntaxTree(unit);
            if (Compiler.errors > 0)
            {
                return;
            }

            var visitor = new SyntaxVisitor(SyntaxTree);
            visitor.Visit();
        }

        private void EnterScope(IScope scope)
        {
            currentScope = scope;
        }

        private void LeaveScope()
        {
            currentScope = currentScope.GetParentScope();
        }

        private ValueType Error(string msg, params object[] pars)
        {
            Console.WriteLine($"  line {Loc.StartLine,3}: {string.Format(msg, pars)}");
            ++Compiler.errors;
            yyerrok();

            CurrentSemanticValue.node = new EmptyNode(Loc);
            CurrentSemanticValue.typeNode = new EmptyTypeNode(Loc);
            CurrentSemanticValue.type = Type.Unknown;

            if (lastErrorToken == Token.Eof)
            {
                YYAbort();
            }

            return CurrentSemanticValue;
        }

        private TypeNode TryCreateOperator(Token token, TypeNode left)
        {
            var expType = left.Type;
            if (!Operator.CanUse(token, expType))
            {
                return Error("Cannot use {0} on {1}.", token, expType)
                       .typeNode;
            }

            return Operator.Create(token, expType, Loc)
                         .WithLeft(left);
        }

        private TypeNode TryCreateOperator(Token token, TypeNode left, TypeNode right)
        {
            if (!Operator.CanUse(token, left.Type, right.Type))
            {
                return Error("Cannot {0} {1} and {2}.", token, left.Type, right.Type.ToString())
                       .typeNode;
            }

            return Operator.Create(token, left.Type, right.Type, Loc)
                         .WithLeft(left)
                         .WithRight(right);
        }

        private TypeNode CreateValue()
        {
            var value = ValueStack[ValueStack.Depth - 1];
            Type type = value.token.ConvertToType();
            string val = value.val;

            if (type == Type.Unknown)
            {
                return Error("Cannot use provided type: {0}", value.token).typeNode;
            }

            return new Value(type, val, CurrentLocationSpan);
        }
    }
}