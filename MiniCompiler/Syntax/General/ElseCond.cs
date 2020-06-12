using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class ElseCond : SingleChildNode<SyntaxNode>
    {
        public ElseCond(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
