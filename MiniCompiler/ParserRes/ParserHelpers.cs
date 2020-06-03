using MiniCompiler.Syntax;
using MiniCompiler.Syntax.General;
using QUT.Gppg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniCompiler
{
    public partial class Parser
    {
        private int level;
        private readonly List<(int id, SyntaxNode node)> childrenWaitingForAdoption;
        private SyntaxTree tree;

        public Parser(Scanner scanner) : base(scanner)
        {
            childrenWaitingForAdoption = new List<(int, SyntaxNode)>();
        }

        public LexLocation Loc => CurrentLocationSpan;

        private void AddOrphan(SyntaxNode node)
        {
            childrenWaitingForAdoption.Add((level, node));
        }

        private void AddChildrenToNode(SyntaxNode node)
        {
            --level;
            node.SetChildren(childrenWaitingForAdoption.Where(pair => pair.id == level + 1).Select(pair => pair.node).ToList());
            childrenWaitingForAdoption.Add((level, node));
        }

        private void GenerateCode(CompilationUnit unit)
        {
            tree = new SyntaxTree(unit);
            var visitor = new SyntaxVisitor(tree);
            visitor.Visit();
        }

        private void Error(string msg)
        {
            Console.WriteLine($"  line {Loc.StartLine,3}: {msg}");
            ++Compiler.errors;
            yyerrok();
        }

        private char BinaryOpGenCode(Tokens t, char type1, char type2)
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
                case Tokens.Plus:
                    Compiler.EmitCode("add");
                    break;
                case Tokens.Minus:
                    Compiler.EmitCode("sub");
                    break;
                case Tokens.Multiplies:
                    Compiler.EmitCode("mul");
                    break;
                case Tokens.Divides:
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
