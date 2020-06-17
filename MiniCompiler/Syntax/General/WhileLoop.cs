using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class WhileLoop : SiblingsNode<TypeNode, SyntaxNode>
    {
        public WhileLoop(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}