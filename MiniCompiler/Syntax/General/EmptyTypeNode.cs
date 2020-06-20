using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.Variables;
using QUT.Gppg;
using System;

namespace MiniCompiler.Syntax.General
{
    public class EmptyTypeNode : TypeNode
    {
        public EmptyTypeNode(LexLocation loc = null)
        {
            Location = loc;
            Type = MiniType.Unknown;
        }

        public override SyntaxNode this[int i] { get => throw new NotImplementedException(); set { } }

        public override int Count => 0;

        public override void Visit(SyntaxVisitor visitor)
        {
        }
    }
}