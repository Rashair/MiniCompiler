using MiniCompiler.Syntax;
using MiniCompiler.Syntax.Declaration.Scopes;
using MiniCompiler.Syntax.General;
using QUT.Gppg;
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
        }

        private char BinaryOpGenCode(Token t, char type1, char type2)
        {
            char type = (type1 == 'i' && type2 == 'i') ? 'i' : 'r';
            if (type1 != type)
            {
                Compiler.EmitCode("stloc temp");
                Compiler.EmitCode("conv.r8");
                Compiler.EmitCode("ldloc temp");
            }
            if (type2 != type)
                Compiler.EmitCode("conv.r8");
            switch (t)
            {
                case Token.Plus:
                    Compiler.EmitCode("add");
                    break;
                case Token.Minus:
                    Compiler.EmitCode("sub");
                    break;
                case Token.Multiplies:
                    Compiler.EmitCode("mul");
                    break;
                case Token.Divides:
                    Compiler.EmitCode("div");
                    break;
                default:
                    Console.WriteLine($"  line {Loc.StartLine,3}:  internal gencode error");
                    ++Compiler.errors;
                    break;
            }
            return type;
        }

    }
}
