using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;
using System;

namespace MiniCompiler.Syntax.General
{
    public class EmptyTypeNode : TypeNode
    {
        public EmptyTypeNode(LexLocation loc = null)
        {
            Location = loc;
        }

        public override SyntaxNode this[int i] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int Count => 0;
    }
}