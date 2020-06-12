using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class IfCond : TrinityNode<TypeNode, SyntaxNode, SyntaxNode>
    {
        public IfCond(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
