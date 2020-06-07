using MiniCompiler.Syntax;
using MiniCompiler.Syntax.Variables.Scopes;
using MiniCompiler.Syntax.General;
using QUT.Gppg;
using MiniCompiler.Syntax.Variables;
using System;

namespace MiniCompiler
{
    public partial class Parser
    {
        private IScope currentScope;

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

        private void Error(string msg, params object[] pars)
        {
            Console.WriteLine($"  line {Loc.StartLine,3}: {string.Format(msg, pars)}");
            ++Compiler.errors;
            yyerrok();
            CurrentSemanticValue.node = new EmptyNode(Loc);
            CurrentSemanticValue.type = Type.Unknown;
        }
    }
}
