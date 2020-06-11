using QUT.Gppg;
using System;

namespace MiniCompiler.Syntax.General
{
    public class EmptyNode : SyntaxNode
    {
        public EmptyNode(LexLocation loc = null) : base(loc)
        {
        }

        public override SyntaxNode this[int i] { get => throw new NotImplementedException(); set { } }

        public override int Count => 0;

        public override bool ShouldInclude => false;
    }
}