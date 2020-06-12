using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.IOStream
{
    public class Write : SingleChildNode<TypeNode>
    {
        public Write(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
